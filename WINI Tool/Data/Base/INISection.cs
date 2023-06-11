// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data.Base
{
    public class INISection : INIContentBase
    {
        private string _name;
        private string _comment;
        private List<INIKey> _iniKeys;
        private List<INIGroup> _iniGroups;
        private bool _useParallelization;
        
        public string Name => _name;
        public string Comment => _comment;
        public List<INIKey> INIKeys => _iniKeys;
        public List<INIGroup> INIGroups => _iniGroups;
        public bool UseParallelization => _useParallelization;

        private INISection(LineContentBase lineContent, string name, string comment) : base(lineContent)
        {
            _name = name;
            _comment = comment;
            
            _iniKeys = new List<INIKey>();
            _iniGroups = new List<INIGroup>();

            _useParallelization = false;
        }

        public static INISection Create(LineContentBase lineContent, string comment = null)
        {
            Match match = RXSectionName.Match(lineContent.Text);

            if (!match.Success)
            {
                Debug.Print(string.Format("INISection::Create({0}) - section doesn't match section format", lineContent.Text));
                return null;
            }
            
            if (match.Groups[0].Length > 0)
            {
                Debug.Print(string.Format("INISection::Create({0}) - section is commented out", lineContent.Text));
                return null;
            }

            string name = match.Groups[1].Value;
            if (string.IsNullOrWhiteSpace(comment))
                comment = match.Groups[5].Value;

            return new INISection(lineContent, name, comment);
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
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.LineContent.PositionStart >= position && inikey.LineContent.PositionEnd <= position && includeCommented ? true : !inikey.IsComment);
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.LineContent.PositionStart >= position && inikey.LineContent.PositionEnd <= position && includeCommented ? true : !inikey.IsComment);
        }

        public long GetPositionFromINIKey(string key, bool includeCommented = false)
        {
            if (_useParallelization)
                return _iniKeys.AsParallel().FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment).LineContent.PositionStart;
            else
                return _iniKeys.FirstOrDefault(inikey => inikey.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) && includeCommented ? true : !inikey.IsComment).LineContent.PositionStart;
        }

        public void AddINIKey(INIKey key)
        {
            if (key != null)
                if (!ContainsKey(key.Key))
                {
                    _iniKeys.Add(key);

                    if (!_useParallelization && _iniKeys.Count > 1000)
                        _useParallelization = true;
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
                Debug.Print(string.Format("INISection::GetValue({0}, {1}) - invalid type", key, defaultValue));
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
                Debug.Print(string.Format("INISection::GetValue({0}, {1}) - invalid type", key, defaultValue));
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
                Debug.Print(string.Format("INISection::GetValue({0}, {1}) - invalid type", key, defaultValue));
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
                Debug.Print(string.Format("INISection::GetValue({0}, {1}) - invalid type", key, defaultValue));
                return defaultValue;
            }

            return bool.Parse(iniKey.Value);
        }
    }
}
