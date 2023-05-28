using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WINI_Tool.Data.Base
{
    // This class expands the INIContentBase class by adding and handling a reference to a section.
    // We expect that only a section will be outside a section.
    public class INISectionContentBase : INIContentBase
    {
        private INISection _originalSection;

        public INISection Section { get; set; }
        
        public INISectionContentBase(long positionStart, string lineContent, INIContentBase previousContent, INIContentBase nextContent = null) : base(positionStart, lineContent, previousContent, nextContent)
        {
            Type previousContentType = previousContent.GetType();

            if (previousContentType == typeof(INIKey))
                _originalSection = ((INIKey)previousContent).Section;
            else if (previousContentType == typeof(INIGroup))
                _originalSection = ((INIGroup)previousContent).Section;
            else if (previousContentType == typeof(INIComment))
                _originalSection = ((INIComment)previousContent).Section;
            else if (previousContentType == typeof(INISection))
                _originalSection = (INISection)previousContent;

            Section = _originalSection;
        }

        public override void Reset()
        {
            Section = _originalSection;
            base.Reset();
        }

        public override void Save()
        {
            _originalSection = Section;
            base.Save();
        }
    }
}
