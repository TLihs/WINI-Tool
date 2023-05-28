using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WINI_Tool.Support
{
    public static class Constants
    {
        // https://regex101.com/r/eNc1SM/1

        private const string _keyValuePairPattern = @"^\s*(;*)\s*([a-zA-Z0-9]+\b)=([^=]*)$";
        private const string _sectionNamePattern = @"^\s*(;*)\s*\[([^;]+)\]\s*(\s*(;+))\s*(.*)$";
        private const string _groupNamePattern = @"^\s*;\s*([^s].+)\s*$";
        private const string _commentPattern = @"^\s*;\s*(.*)\s*$";
        private const string _leadingWhiteSpace = @"^\s+(.*)$";
        private const string _followingWhiteSpace = @"^(.*)\s+$";

        public static Regex RXKeyValuePair = new Regex(_keyValuePairPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXSectionName = new Regex(_sectionNamePattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXGroupName = new Regex(_groupNamePattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXComment = new Regex(_commentPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXLeadingWhiteSpace = new Regex(_leadingWhiteSpace, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
        public static Regex RXFollowingWhiteSpace = new Regex(_followingWhiteSpace, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
    }
}
