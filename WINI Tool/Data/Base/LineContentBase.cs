﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

using static WINI_Tool.Support.Constants;

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
        
        private long _positionStart;
        private long _positionEnd;
        private long _lineNumber;
        private string _originalText;
        private string _text;
        private LineContentBase _nextContent;
        private LineContentBase _previousContent;
        private int _errorList;
        private INIContentType _contentType;

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

        public LineContentBase(long positionStart, string lineContent, LineContentBase previousContent, LineContentBase nextContent = null)
        {
            _errorList = 0x0;

            PreviousContent = previousContent;
            NextContent = nextContent;

            _originalText = lineContent;
            PositionStart = positionStart;
            Text = _originalText;
        }

        private void EvaluateContent()
        {
            if (RXSectionName.IsMatch(_text))
                _contentType = INIContentType.Section;
            else if (RXGroupName.IsMatch(_text))
                _contentType = INIContentType.Group;
            else if (RXKeyValuePair.IsMatch(_text))
                _contentType = INIContentType.Key;
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
                    if (RXSemiColon.IsMatch(_text))
                        _errorList |= ERR_LINECONTENT_INVALIDCHAR;
                    break;

                case INIContentType.Group:
                    if (RXSemiColon.IsMatch(_text))
                        _errorList |= ERR_LINECONTENT_INVALIDCHAR;
                    break;

                case INIContentType.Key:
                    if (RXSemiColon.IsMatch(_text))
                        _errorList |= ERR_LINECONTENT_INVALIDCHAR;
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
    }
}