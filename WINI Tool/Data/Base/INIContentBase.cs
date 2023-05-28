using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data.Base
{
    public abstract class INIContentBase
    {
        public enum INIContentType
        {
            Empty = 0,
            Section = 1,
            Group = 2,
            Key = 3,
            Comment = 4
        }
        
        private long _positionStart;
        private long _positionEnd;
        private string _originalLineContent;
        private string _lineContent;
        private INIContentBase _nextContent;
        private INIContentBase _previousContent;
        private List<int> _errorList;
        private INIContentType _contentType;

        // We expect PositionStart to not change, unless a previous content is changed.
        public long PositionStart
        {
            get => _positionStart;
            set
            {
                if (_positionStart != value)
                {
                    _positionStart = value;
                    // By setting PositionEnd, PositionStart of the next content will be refreshed.
                    PositionEnd = _positionStart + _lineContent.Length;
                }
            }
        }
        // We expect PositionEnd to change, when previous or this content changes.
        public long PositionEnd
        {
            get => _positionEnd;
            set
            {
                if (_positionEnd != value)
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
        public INIContentBase PreviousContent
        {
            get => _previousContent;
            set
            {
                if (_previousContent != value)
                {
                    _previousContent = value;

                    // By changing PositionStart, PositionEnd will automatically be changed.
                    if (_previousContent != null)
                        PositionStart = _previousContent.PositionEnd + 1;
                }
            }
        }
        public INIContentBase NextContent
        {
            get => _nextContent;
            set
            {
                if (_nextContent != value)
                {
                    _nextContent = value;

                    // All necessary steps to refresh the next content is done by the next
                    // content itself.
                    if (_nextContent != null)
                        _nextContent.PreviousContent = this;
                }
            }
        }
        public string LineContent
        {
            get => _lineContent;
            set
            {
                if (_lineContent != value)
                {
                    _lineContent = value;
                    // By setting PositionEnd, PositionStart of the next content will be refreshed.
                    PositionEnd = PositionStart + value.Length;
                    EvaluateContent();
                }
            }
        }
        public long Length => LineContent.Length;
        public List<int> ErrorList => _errorList;
        public INIContentType ContentType => _contentType;

        public INIContentBase(long positionStart, string lineContent, INIContentBase previousContent, INIContentBase nextContent = null)
        {
            _errorList = new List<int>();

            PreviousContent = previousContent;
            NextContent = nextContent;

            _originalLineContent = lineContent;
            PositionStart = positionStart;
            LineContent = _originalLineContent;
        }

        private void EvaluateContent()
        {
            if (RXSectionName.IsMatch(_lineContent))

            EvaluateErrors();
        }

        private void EvaluateErrors()
        {
            switch (_contentType)
            {
                case INIContentType.Empty:
                    _errorList.Clear();
                    break;
            }
        }

        public virtual void Reset()
        {
            // We don't change PositionEnd or the next contents PositionStart actively, but
            // indirectly when resetting the LineContent.
            LineContent = _originalLineContent;
        }

        public virtual void Save()
        {
            // We don't have to refresh PositionEnd, since it does not change, because the
            // LineContent ist not changed, only the _originalLineContent.
            _originalLineContent = LineContent;
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
    }
}
