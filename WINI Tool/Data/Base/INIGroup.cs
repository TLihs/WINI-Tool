// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        private INIGroup(LineContentBase lineContent, string groupName) : base(lineContent)
        {
            _groupName = groupName;
        }

        public static INIGroup Create(LineContentBase lineContent)
        {
            if (!RXGroupName.IsMatch(lineContent.Text))
            {
                Debug.Print(string.Format("INIGroup::Create({0}) - content doesn't match format", lineContent.Text));
                return null;
            }
            
            Match match = RXGroupName.Match(lineContent.Text);
            string groupname = match.Groups[0].Value;

            if (string.IsNullOrWhiteSpace(groupname))
                return null;
            else
                return new INIGroup(lineContent, groupname);
        }

        // Keys should be added via the LineContentBase constructor by back looping to the next available group within a section
        public void AddKey(INIKey key)
        {

        }
    }
}
