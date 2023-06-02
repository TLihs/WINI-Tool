using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

using static WINI_Tool.Support.Constants;
using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Base
{
    public class LineContentBase
    {
        public enum INIContentType
        {
            Empty = 0,
            Section = 1,
            Group = 2,
            Key = 3,
            Comment = 4
        }

        private INIReader _reader;
        private INIContentBase _iniContent;
        private long _positionStart;
        private long _positionEnd;
        private long _lineNumber;
        private string _originalText;
        private string _text;
        private LineContentBase _nextContent;
        private LineContentBase _previousContent;
        private int _errorList;
        private INIContentType _contentType;

        public INIReader Reader => _reader;        
        // We expect PositionStart to not change, unless a previous content is changed.
        public long PositionStart
        {
            get => _positionStart;
            set
            {
                if (value != _positionStart)
                {
                    _positionStart = value;
                    // By setting PositionEnd, PositionStart of the next content will be refreshed.
                    PositionEnd = _positionStart + _text.Length;
                }
            }
        }
        // We expect PositionEnd to change, when previous or this content changes.
        public long PositionEnd
        {
            get => _positionEnd;
            set
            {
                if (value != _positionEnd)
                {
                    long tempEnd = _positionEnd;
                    _positionEnd = value;

                    // All necessary steps to refresh the next content is done by the next
                    // content itself.
                    if (NextContent != null)
                        NextContent.PositionStart = (tempEnd - value) + 1;
                }
            }
        }
        public LineContentBase PreviousContent
        {
            get => _previousContent;
            set
            {
                if (value != _previousContent)
                {
                    _previousContent = value;

                    // By changing PositionStart, PositionEnd will automatically be changed.
                    if (_previousContent != null)
                        PositionStart = _previousContent.PositionEnd + 1;
                }
            }
        }
        public LineContentBase NextContent
        {
            get => _nextContent;
            set
            {
                if (value != _nextContent)
                {
                    _nextContent = value;

                    // All necessary steps to refresh the next content is done by the next
                    // content itself.
                    if (_nextContent != null)
                        _nextContent.PreviousContent = this;
                }
            }
        }
        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    // By setting PositionEnd, PositionStart of the next content will be refreshed.
                    PositionEnd = PositionStart + value.Length;
                    EvaluateContent();
                }
            }
        }
        public long LineNumber
        {
            get => _lineNumber;
            set
            {
                if (value != _lineNumber)
                {
                    _lineNumber = value;
                    if (_nextContent != null)
                        _nextContent.LineNumber = _lineNumber + 1;
                }
            }
        }
        public long Length => _text.Length;
        public int Errors => _errorList;
        public INIContentType ContentType => _contentType;

        public LineContentBase(INIReader reader, long positionStart, string lineContent, LineContentBase previousContent = null, LineContentBase nextContent = null)
        {
            _errorList = 0x0;

            _reader = reader;

            PreviousContent = previousContent;
            NextContent = nextContent;

            _originalText = lineContent;
            PositionStart = positionStart;
            Text = _originalText;
        }

        private void EvaluateContent()
        {
            if (RXSectionName.IsMatch(_text))
            {
                _contentType = INIContentType.Section;
                _iniContent = INISection.Create(this);
            }
            else if (RXGroupName.IsMatch(_text))
            {
                _contentType = INIContentType.Group;
                _iniContent = INIGroup.Create(this);
            }
            else if (RXKeyValuePair.IsMatch(_text))
            {
                _contentType = INIContentType.Key;
                _iniContent = INIKey.Create(this);
            }
            else if (RXComment.IsMatch(_text))
                _contentType = INIContentType.Comment;
            else
                _contentType = INIContentType.Empty;

            EvaluateErrors();
        }

        private void EvaluateErrors()
        {
            _errorList = 0x0;

            switch (_contentType)
            {
                case INIContentType.Empty:
                    if (!string.IsNullOrWhiteSpace(_text))
                        _errorList |= ERR_LINECONTENT_NOMATCH;
                    break;

                case INIContentType.Section:
                    if (RXInvalidChars.IsMatch(_text))
                        _errorList |= ERR_SECTION_INVALIDCHAR;
                    break;

                case INIContentType.Group:
                    if (RXInvalidChars.IsMatch(_text))
                        _errorList |= ERR_GROUP_INVALIDCHAR;
                    break;

                case INIContentType.Key:
                    if (RXInvalidKeyChars.IsMatch(_text))
                        _errorList |= ERR_KEY_INVALIDCHAR;
                    break;

                case INIContentType.Comment:
                    break;
            }
        }

        public virtual void Reset()
        {
            // We don't change PositionEnd or the next contents PositionStart actively, but
            // indirectly when resetting the LineContent.
            Text = _originalText;
        }

        public virtual void Save()
        {
            // We don't have to refresh PositionEnd, since it does not change, because the
            // LineContent ist not changed, only the _originalText.
            _originalText = Text;
        }

        public void Destroy()
        {
            // We have to manually bridge over this by linking previous and next content
            // directly together. Using the Destructor of this object would be unwise,
            // since the destructor is called, when GC will destroy it, which isn't,
            // when we actually want to get rid of this object.
            if (PreviousContent != null)
                PreviousContent.NextContent = NextContent;
            else if (NextContent != null)
                NextContent.PreviousContent = PreviousContent;
        }

        public void MoveAfter(LineContentBase previousContent)
        {
            Destroy();
            PreviousContent = previousContent;
            NextContent = PreviousContent.NextContent;

            if (NextContent != null)
                NextContent.PreviousContent = this;
        }

        public INISection GetSection(string sectionName)
        {
            if (_iniContent.GetType() == typeof(INISection) && ((INISection)_iniContent).Name.Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                return (INISection)_iniContent;
            else if (NextContent == null)
                return null;
            else
                return NextContent.GetSection(sectionName);
        }

        public INISection GetCurrentSection()
        {
            if (_iniContent.GetType() == typeof(INISection))
                return (INISection)_iniContent;
            else if (PreviousContent == null)
                return null;
            else
                return PreviousContent.GetCurrentSection();
        }

        public INIGroup GetCurrentGroup()
        {
            if (_iniContent.GetType() == typeof(INIGroup))
                // found group
                return (INIGroup)_iniContent;
            else if (_iniContent.GetType() == typeof(INISection))
                // if we reached the declaration of the INI section, then the calling content isn't within a group
                return null;
            else if (PreviousContent == null)
                // if we reached the beginning of the INI file, then the calling content isn't within a group
                return null;
            else
                // try the next previous content
                return PreviousContent.GetCurrentGroup();
        }
    }
}
