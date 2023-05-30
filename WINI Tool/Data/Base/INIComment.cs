using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WINI_Tool.Data.Base.Base
{
    public abstract class INIComment : INIContentBase
    {
        private string _originalComment;

        public string Comment { get; set; }
        public long LineNumber { get; set; }

        public INIComment(long positionStart, string comment, LineContentBase lineContent, LineContentBase referenceContent = null) : base(lineContent)
        {
            _originalComment = comment;
        }
    }
}
