using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WINI_Tool.Data.Base;

namespace WINI_Tool.Data
{
    public abstract class INIComment : INIContentBase
    {
        private string _originalComment;

        public string Comment { get; set; }
        public long LineNumber { get; set; }

        public INIComment(long positionStart, string comment, LineContentBase previousContent) : base(positionStart, comment, previousContent)
        {
            _originalComment = comment;
        }

        public override void Reset()
        {
            Comment = _originalComment;
            base.Reset();
        }

        public override void Save()
        {
            _originalComment = Comment;
            base.Save();
        }
    }

    public class INICommentKey : INIComment
    {
        private INIKey _originalKey;
        
        public INIKey Key { get; set; }
        
        public INICommentKey(long positionStart, string comment, LineContentBase previousContent, INIKey key) : base(positionStart, comment, previousContent)
        {
            _originalKey = key;
            Key = _originalKey;
        }

        public override void Reset()
        {
            Key = _originalKey;
            base.Reset();
        }

        public override void Save()
        {
            _originalKey = Key;
            base.Save();
        }
    }

    public class INICommentSection : INIComment
    {
        private INISection _originalSection;

        public INISection Section { get; set; }
        
        public INICommentSection(long positionStart, string comment, LineContentBase previousContent, INISection section) : base(positionStart, comment, previousContent)
        {
            _originalSection = section;
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
