using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WINI_Tool.Data.Base
{
    public class INIContentBase
    {
        private LineContentBase _lineContent;

        public LineContentBase LineContent => _lineContent;
        
        public INIContentBase(LineContentBase lineContent)
        {
            _lineContent = lineContent;
        }

        public INISection GetCurrentSection()
        {
            return _lineContent.GetCurrentSection();
        }

        public INIGroup GetCurrentGroup()
        {
            return _lineContent.GetCurrentGroup();
        }
    }
}
