using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

using WINI_Tool.Data.Base;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data
{
    public class INISection : INIContentBase
    {
        private string _originalName;
        private string _originalComment;
        private List<INIKey> _iniKeys;
        private List<INIGroup> _iniGroups;
        private INICommentSection _sectionComment;
        private bool _useParallelization;
        
        public string Name { get; set; }
        public string Comment { get; set; }

        private INISection(long positionStart, string name, string lineContent, INIContentBase previousContent, string comment) : base(positionStart, lineContent, previousContent)
        {
            _originalName = name;
            _originalComment = comment == null ? string.Empty : comment;
            _iniKeys = new List<INIKey>();
            _useParallelization = false;

            Reset();
        }

        public static INISection Create(long positionStart, string lineContent, INIContentBase previousContent, string comment = null)
        {
            if (!RXSectionName.IsMatch(lineContent))
            {
                Debug.Print(string.Format("INISection::Create(%d, %s, ..., %s) - section doesn't match section format", positionStart, lineContent, comment == null ? string.Empty : comment));
                return null;
            }
            
            Match match = RXSectionName.Match(lineContent);
            if (match.Groups[0].Length > 0)
            {
                Debug.Print(string.Format("INISection::Create(%d, %s, ..., %s) - section is commented out", positionStart, lineContent, comment == null ? string.Empty : comment));
                return null;
            }

            string name = match.Groups[1].Value;
            // TODO: Probably not necessary since the RegEx doesn't match empty content.
            //if (string.IsNullOrWhiteSpace(name))
            //{
            //    Debug.Print(string.Format("INISection::Create(%d, %s, ..., %s) - section name is null or whitespace", positionStart, lineContent, comment == null ? string.Empty : comment));
            //    return null;
            //}

            return new INISection(positionStart, name, lineContent, previousContent, comment);
        }

        public bool ContainsKey(string key, bool includeCommented = false)
        {
            if (_useParallelization)
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment) != null;
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment) != null;
        }

        public INIKey GetINIKey(string key, bool includeCommented = false)
        {
            if (_useParallelization)
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment);
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment);
        }

        public INIKey GetINIKeyFromPosition(long position, bool includeCommented = false)
        {
            if (_useParallelization)
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.PositionStart >= position && inikey.PositionEnd <= position && includeCommented ? true : !inikey.IsComment);
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.PositionStart >= position && inikey.PositionEnd <= position && includeCommented ? true : !inikey.IsComment);
        }

        public long GetPositionFromINIKey(string key, bool includeCommented = false)
        {
            if (_useParallelization)
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment).PositionStart;
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment).PositionStart;
        }

        public void AddKeyValuePair(long positionStart, string lineContent)
        {
            if (!ContainsKey(key))
            {
                INIKey inikey = INIKey.Create(positionStart, lineContent, value, line, isComment, lineContent);
                if (inikey != null)
                {
                    _iniKeys.Add(inikey);

                    if (!_useParallelization && _iniKeys.Count > 1000)
                        _useParallelization = true;
                }
            }
        }

        public void AddGroup(long positionStart, string lineContent, string groupName)
        {

        }

        public void RemoveKeyValuePair(long position, bool includeCommented = false)
        {
            INIKey key = GetINIKeyFromPosition(position, includeCommented);

            if (key != null)
                if (_iniKeys.Remove(key))
                    if (_useParallelization && _iniKeys.Count < 1000)
                        _useParallelization = false;
        }

        public void RemoveKeyValuePair(string key)
        {
            if (ContainsKey(key))
            {
                if (_iniKeys.Remove(GetINIKey(key)))
                    if (_useParallelization && _iniKeys.Count < 1000)
                        _useParallelization = false;
            }
        }

        public long GetValue(string key, long defaultValue)
        {
            INIKey iniKey = GetINIKey(key);

            if (iniKey == null)
                return defaultValue;

            if (!iniKey.IsOfType(defaultValue.GetType()))
            {
                Debug.Print(string.Format("INISection::GetValue(%s, %d) - invalid type", key, defaultValue));
                return defaultValue;
            }

            return long.Parse(iniKey.Value);
        }

        public double GetValue(string key, double defaultValue)
        {
            INIKey iniKey = GetINIKey(key);

            if (iniKey == null)
                return defaultValue;

            if (!iniKey.IsOfType(defaultValue.GetType()))
            {
                Debug.Print(string.Format("INISection::GetValue(%s, %d) - invalid type", key, defaultValue));
                return defaultValue;
            }

            return double.Parse(iniKey.Value);
        }

        public string GetValue(string key, string defaultValue)
        {
            INIKey iniKey = GetINIKey(key);

            if (iniKey == null)
                return defaultValue;

            if (!iniKey.IsOfType(defaultValue.GetType()))
            {
                Debug.Print(string.Format("INISection::GetValue(%s, %d) - invalid type", key, defaultValue));
                return defaultValue;
            }

            return iniKey.Value;
        }

        public bool GetValue(string key, bool defaultValue)
        {
            INIKey iniKey = GetINIKey(key);

            if (iniKey == null)
                return defaultValue;

            if (!iniKey.IsOfType(defaultValue.GetType()))
            {
                Debug.Print(string.Format("INISection::GetValue(%s, %d) - invalid type", key, defaultValue));
                return defaultValue;
            }

            return bool.Parse(iniKey.Value);
        }

        public override void Reset()
        {
            Name = _originalName;
            Comment = _originalComment;

            base.Reset();
        }

        public void ResetContent()
        {
            if (_iniKeys.Count > 0)
                _iniKeys[0].Reset();
        }

        public void ResetAll()
        {
            Reset();
            ResetContent();
        }

        public override void Save()
        {
            _originalName = Name;
            _originalComment = Comment;

            base.Save();
        }

        public void SaveContent()
        {
            if (_iniKeys.Count > 0)
                _iniKeys[0].Save();
        }

        public void SaveAll()
        {
            Save();
            SaveContent();
        }
    }
}
