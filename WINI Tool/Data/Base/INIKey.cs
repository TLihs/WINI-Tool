using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

                }
            }
        }
        public bool IsComment
        {
            get => _isComment;
            set
            {

            }
        }

        private INIKey(LineContentBase lineContent, string key, string value, bool isComment) : base(lineContent)
        {
            _key = key;
            _value = value;
            _isComment = isComment;

            Reset();
        }

        public static INIKey Create(LineContentBase lineContent, string key, string value, bool isComment)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.Print(string.Format("INISection::Create(%s, %s, %s, %s) - key is null or whitespace", lineContent, key, value, isComment ? "as comment" : "not as comment"));
                return null;
            }

            return new INIKey(lineContent, key, value, isComment);
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

            Debug.Print(string.Format("INIKeys::IsOfType(%s) - Not a valid type.", type.FullName));
            return false;
        }
    }
}
