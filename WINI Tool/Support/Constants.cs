// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.IO;
using System.Text.RegularExpressions;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Support
{
    public static class Constants
    {
        // https://regex101.com/r/eNc1SM/1

        private const string _keyValuePairPattern = @"^\s*(;*)\s*(.*)=(.*)$";
        private const string _sectionNamePattern = @"^\s*(;*)\s*\[(.*)\]\s*(\s*(;*))\s*(.*)$";
        private const string _groupNamePattern = @"^\s*;[g]\s+(.+)\s*$";
        private const string _commentPattern = @"^\s*;\s*(.*)\s*$";
        private const string _leadingWhiteSpace = @"^\s+(.*)$";
        private const string _followingWhiteSpace = @"^(.*)\s+$";
        private const string _invalidCharsPattern = @"[^a-zA-Z0-9]";
        private const string _invalidKeyCharsPattern = @"(" + _invalidCharsPattern + ".*)+=(.*)";

        public static Regex RXKeyValuePair = new Regex(_keyValuePairPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXSectionName = new Regex(_sectionNamePattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXGroupName = new Regex(_groupNamePattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXComment = new Regex(_commentPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXLeadingWhiteSpace = new Regex(_leadingWhiteSpace, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXFollowingWhiteSpace = new Regex(_followingWhiteSpace, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXInvalidChars = new Regex(_invalidCharsPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXInvalidKeyChars = new Regex(_invalidKeyCharsPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

        public static class Typing
        {
            public enum KeyValueTypes
            {
                Invalid = -1,
                String = 0,
                Bool = 1,
                Int = 2,
                Float = 3,
                Path = 4,
                DateTime = 5
            }

            public static KeyValueTypes BaseTypeToINIType(Type type)
            {
                if (type == typeof(bool))
                    return KeyValueTypes.Bool;

                if (type == typeof(byte))
                    return KeyValueTypes.Int;
                if (type == typeof(short))
                    return KeyValueTypes.Int;
                if (type == typeof(ushort))
                    return KeyValueTypes.Int;
                if (type == typeof(long))
                    return KeyValueTypes.Int;
                if (type == typeof(ulong))
                    return KeyValueTypes.Int;

                if (type == typeof(float))
                    return KeyValueTypes.Float;
                if (type == typeof(double))
                    return KeyValueTypes.Float;
                if (type == typeof(decimal))
                    return KeyValueTypes.Float;

                if (type == typeof(DateTime))
                    return KeyValueTypes.DateTime;
                if (type == typeof(TimeSpan))
                    return KeyValueTypes.DateTime;

                if (type == typeof(File))
                    return KeyValueTypes.Path;
                if (type == typeof(Directory))
                    return KeyValueTypes.Path;
                if (type == typeof(FileInfo))
                    return KeyValueTypes.Path;
                if (type == typeof(DirectoryInfo))
                    return KeyValueTypes.Path;

                if (type == typeof(string))
                    return KeyValueTypes.String;

                return KeyValueTypes.Invalid;
            }

            public static Type INITypeToBaseType(KeyValueTypes type)
            {
                switch (type)
                {
                    case KeyValueTypes.Bool:
                        return typeof(bool);
                    case KeyValueTypes.Float:
                        return typeof(double);
                    case KeyValueTypes.Path:
                        return typeof(FileInfo);
                    case KeyValueTypes.String:
                        return typeof(string);
                    case KeyValueTypes.DateTime:
                        return typeof(DateTime);
                    case KeyValueTypes.Invalid:
                        Log(EXCEPTIONTYPES.ERR_TYPING_INITYPEINVALID, string.Format("Type is 'INVALID'. Parsing as string recommended."));
                        return typeof(string);
                    default:
                        Log(EXCEPTIONTYPES.ERR_TYPING_INITYPEINOTIMPLEMENTED, string.Format("type= '{0}'", Enum.GetName(typeof(KeyValueTypes), type)));
                        return null;
                }
            }
        }
    }
}
