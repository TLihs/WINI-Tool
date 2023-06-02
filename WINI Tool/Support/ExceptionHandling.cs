using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Support
{
    public static class ExceptionHandling
    {
        private static string _logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WINI Tool", "Log", "debug.log");
        private static StreamWriter _logFileStream;
        
        public const int ERR_LINECONTENT_NOMATCH = 0x_0001;
        public const int ERR_SECTION_NOMATCH = 0x_0002;
        public const int ERR_GROUP_NOMATCH = 0x_0004;
        public const int ERR_KEY_NOMATCH = 0x_0006;
        public const int ERR_SECTION_INVALIDCHAR = 0x_0008;
        public const int ERR_GROUP_INVALIDCHAR = 0x_0010;
        public const int ERR_KEY_INVALIDCHAR = 0x_0020;

        public const int ERR_FUNCTION_NOTIMPLEMENTED = 0x_0040;

        public const int ERR_TYPING_CSTYPEINVALID = 0x_0060;

        public static string LogFilePath => _logFilePath;

        static ExceptionHandling()
        {
            FileInfo fileinfo = new FileInfo(_logFilePath);
            Directory.CreateDirectory(fileinfo.DirectoryName);
            if (!fileinfo.Exists)
                File.WriteAllText(fileinfo.FullName, ": Log created.");
            else
                File.AppendAllText(fileinfo.FullName, ": Log opened.");

            _logFileStream = new StreamWriter(_logFilePath);
            Debug.Listeners.Add(new TextWriterTraceListener(_logFileStream));
            Debug.Print(DateTime.Now.ToString() + ": Exception Handling initialized.");
        }

        private static string ErrorToString(int error)
        {
            List<string> errors = new List<string>();
            
            if ((error & ERR_LINECONTENT_NOMATCH) == ERR_LINECONTENT_NOMATCH)
                errors.Add("line content does not match any regular expression");
            if ((error & ERR_SECTION_INVALIDCHAR) == ERR_SECTION_INVALIDCHAR)
                errors.Add("section name contains illegal char");
            if ((error & ERR_GROUP_INVALIDCHAR) == ERR_GROUP_INVALIDCHAR)
                errors.Add("group name contains illegal char");
            if ((error & ERR_KEY_INVALIDCHAR) == ERR_KEY_INVALIDCHAR)
                errors.Add("key name contains illegal char");

            if ((error & ERR_FUNCTION_NOTIMPLEMENTED) == ERR_FUNCTION_NOTIMPLEMENTED)
                errors.Add("function not implemented");

            if ((error & ERR_TYPING_CSTYPEINVALID) == ERR_TYPING_CSTYPEINVALID)
                errors.Add("type not implemented");

            return string.Join("\r\n", errors);
        }

        public static void LogFault(int errorCode, string additionalInfo = null)
        {
            Debug.Print(DateTime.Now.ToString() + ": " 
                + (string.IsNullOrEmpty(additionalInfo) ? "" : " - " + additionalInfo)
                + ErrorToString(errorCode)
                + "(" + string.Format("0x{%d}", errorCode.ToString("X4")) + ")");
        }
        
        private static void SafeThrowException(Exception ex)
        {
            
        }

        private static void LogException(Exception ex)
        {

        }
    }
}
