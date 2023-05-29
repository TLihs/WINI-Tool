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
        public const int ERR_LINECONTENT_INVALIDCHAR = 0x_0002;

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
            
            if ((error & ERR_LINECONTENT_NOMATCH) != 0)
                errors.Add("line content does not match regular expression");
            if ((error & ERR_LINECONTENT_INVALIDCHAR) != 0)
                errors.Add("line content contains illegal char");

            return string.Join(";", errors);
        }

        public static void LogFault(int errorCode)
        {
            Debug.Print(DateTime.Now.ToString() + ": " + ErrorToString(errorCode) + "(" + string.Format("0x{0}", errorCode.ToString("X4")) + ")");
        }
        
        private static void SafeThrowException(Exception ex)
        {
            
        }

        private static void LogException(Exception ex)
        {

        }
    }
}
