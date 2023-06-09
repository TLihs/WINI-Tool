using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data.Base
{
    public class INIKey : INIContentBase
    {
        private INIGroup _iniGroup;
        private string _key;
        private string _value;
        private bool _isComment;
        
        public INIGroup Group => _iniGroup;
        public string Key
        {
            get => _key;
            set
            {
                if (value != _key)
                {
                    _key = value;
                }
            }
        }
        public string Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                }
            }
        }
        public bool IsComment
        {
            get => _isComment;
            set
            {
                if (value != _isComment)
                {
                    _isComment = value;
                }
            }
        }

        private INIKey(LineContentBase lineContent, string key, string value, bool isComment) : base(lineContent)
        {
            _key = key;
            _value = value;
            _isComment = isComment;

            Reset();
        }

        public static INIKey Create(LineContentBase lineContent)
        {
            Match match = RXSectionName.Match(lineContent.Text);

            if (!match.Success)
            {
                Debug.Print(string.Format("INIKey::Create({0}) - key doesn't match key format", lineContent.Text));
                return null;
            }

            string key = match.Groups[1].Value;
            string value = match.Groups[2].Value;
            bool iscomment = match.Groups[0].Length > 0;
            
            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.Print(string.Format("INIKey::Create({0}, {1}, {2}, {3}) - key is null or whitespace", lineContent, key, value, iscomment ? "as comment" : "not as comment"));
                return null;
            }

            return new INIKey(lineContent, key, value, iscomment);
        }

        public string GetINIFormatted()
        {
            return (IsComment ? ";" : string.Empty) + Key + "=" + Value;
        }

        public void Reset()
        {
            LineContent.Reset();
        }

        public void Save()
        {
            LineContent.Save();
        }

        public bool IsOfType(Type type)
        {
            if (type == typeof(string))
                return true;

            if (type == typeof(int))
            {
                int outvalue;
                return int.TryParse(Value, out outvalue);
            }

            if (type == typeof(long))
            {
                long outvalue;
                return long.TryParse(Value, out outvalue);
            }

            if (type == typeof(byte))
            {
                byte outvalue;
                return byte.TryParse(Value, out outvalue);
            }

            if (type == typeof(double))
            {
                double outvalue;
                return double.TryParse(Value, out outvalue);
            }

            if (type == typeof(float))
            {
                float outvalue;
                return float.TryParse(Value, out outvalue);
            }

            if (type == typeof(bool))
            {
                bool outvalue;
                return bool.TryParse(Value, out outvalue);
            }

            Debug.Print(string.Format("INIKeys::IsOfType({0}) - Not a valid type.", type.FullName));
            return false;
        }
    }
}
