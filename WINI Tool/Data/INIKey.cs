using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using WINI_Tool.Data.Base;

namespace WINI_Tool.Data
{
    public class INIKey : INIContentBase
    {
        private INIGroup _originalGroup;
        private string _originalLineContent;
        private string _originalKey;
        private string _originalValue;
        private bool _originalIsComment;
        
        public INIGroup Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsComment { get; set; }

        private INIKey(long positionStart, string lineContent, LineContentBase previousContent, string key, string value, bool isComment) : base(positionStart, lineContent, previousContent)
        {
            _originalLineContent = lineContent;
            _originalKey = key;
            _originalValue = value;
            _originalIsComment = isComment;

            Reset();
        }

        public static INIKey Create(long positionStart, string lineContent, LineContentBase previousContent, string key, string value, bool isComment)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.Print(string.Format("INISection::Create(%d, %s, ..., %s, %s, %s) - key is null or whitespace", positionStart, lineContent, key, value, isComment ? "as comment" : "not as comment", lineContent));
                return null;
            }

            return new INIKey(positionStart, lineContent, previousContent, key, value, isComment);
        }
        public string GetINIFormatted()
        {
            return (IsComment ? ";" : string.Empty) + Key + "=" + Value;
        }

        public override void Reset()
        {
            Group = _originalGroup;
            LineContent = _originalLineContent;
            Key = _originalKey;
            Value = _originalValue;
            IsComment = _originalIsComment;

            base.Reset();
        }

        public override void Save()
        {
            _originalGroup = Group;
            _originalLineContent = LineContent;
            _originalKey = Key;
            _originalValue = Value;
            _originalIsComment = IsComment;

            base.Save();
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

            Debug.Print(string.Format("INIKeys::IsOfType(%s) - Not a valid type.", type.FullName));
            return false;
        }
    }
}
