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
    public class INIGroup : INISectionContentBase
    {
        private string _originalGroupName;

        public string GroupName { get; set; }

        private INIGroup(long positionStart, string lineContent, INIContentBase previousContent, string groupName) : base(positionStart, lineContent, previousContent)
        {
            _originalGroupName = groupName;
            GroupName = _originalGroupName;
        }

        public static INIGroup Create(long positionStart, string lineContent, INIContentBase previousContent)
        {
            if (!RXGroupName.IsMatch(lineContent))
            {
                Debug.Print(string.Format("INIGroup::Create(%d, %s, ...) - content doesn't match format", positionStart, lineContent));
                return null;
            }
            
            Match match = RXGroupName.Match(lineContent);
            if (string.IsNullOrWhiteSpace(groupName))
                return null;
            else
                return new INIGroup(positionStart, lineContent, previousContent, groupName);
        }

        public override void Reset()
        {
            GroupName = _originalGroupName;
            base.Reset();
        }

        public override void Save()
        {
            _originalGroupName = GroupName;
            base.Save();
        }
    }
}
