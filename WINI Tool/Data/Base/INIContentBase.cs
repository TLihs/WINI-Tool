using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Data.Base
{
    public abstract class INIContentBase
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

        protected virtual void EvaluateContent()
        {
            LogFault(ERR_FUNCTION_NOTIMPLEMENTED, "INIContentBase::EvaluateContent()");
        }
    }
}
