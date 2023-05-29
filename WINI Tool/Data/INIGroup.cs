using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using WINI_Tool.Data.Base;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data
{
    public class INIGroup : INIContentBase
    {
        private string _groupName;
        private List<INIKey> _keys;

        public string GroupName => _groupName;
        public List<INIKey> Keys => _keys;

        private INIGroup(LineContentBase lineContent, INISection section, string groupName) : base(lineContent)
        {
            _groupName = groupName;
        }

        public static INIGroup Create(LineContentBase lineContent, INISection section)
        {
            if (!RXGroupName.IsMatch(lineContent.Text))
            {
                Debug.Print(string.Format("INIGroup::Create(%s) - content doesn't match format", lineContent.Text));
                return null;
            }
            
            Match match = RXGroupName.Match(lineContent.Text);
            string groupname = match.Groups[0].Value;

            if (string.IsNullOrWhiteSpace(groupname))
                return null;
            else
                return new INIGroup(lineContent, section, groupname);
        }
    }
}
