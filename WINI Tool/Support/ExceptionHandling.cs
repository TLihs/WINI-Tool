using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using static WPFExceptionHandler.ExceptionManagement;

namespace WINI_Tool.Support
{
    public static class ExceptionHandling
    {
        private static string _logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WINI Tool", "Log", "debug.log");
        private static StreamWriter _logFileStream;
        
        public enum WT_ERROR : uint
        {
            WT_E_FAULTNOTIMPLEMENTED = 0x_80000000,

            WT_E_LINECONTENT_NOMATCH = 0x_80000001,
            WT_E_SECTION_NOMATCH = 0x_80000002,
            WT_E_GROUP_NOMATCH = 0x_80000003,
            WT_E_KEY_NOMATCH = 0x_80000004,

            WT_E_SECTION_INVALIDCHAR = 0x_80000005,
            WT_E_GROUP_INVALIDCHAR = 0x_80000010,
            WT_E_KEY_INVALIDCHAR = 0x_80000011,
            WT_E_FUNCTION_NOTIMPLEMENTED = 0x_80000012,

            WT_E_TYPING_CSTYPEINVALID = 0x_80000100,
            WT_E_TYPING_INITYPEINVALID = 0x_80000101,
            WT_E_TYPING_INITYPEINOTIMPLEMENTED = 0x_80000102
        }

    public static string LogFilePath => _logFilePath;

        static ExceptionHandling()
        {

        }

        public static void Create(string logPath = "")
        {
            Debug.Print("Initializing ExceptionHandling...");
            CreateExceptionManagement(App.Current, AppDomain.CurrentDomain, true);
            Debug.Print("ExceptionHandling initialized.");
        }

        private static string ErrorToString(WT_ERROR error)
        {
            switch (error)
            {
                case WT_ERROR.WT_E_LINECONTENT_NOMATCH:
                    return "Line content does not match any regular expression.";
                case WT_ERROR.WT_E_SECTION_INVALIDCHAR:
                    return "Section name contains illegal char.";
                case WT_ERROR.WT_E_GROUP_INVALIDCHAR:
                    return "Group name contains illegal char.";
                case WT_ERROR.WT_E_KEY_INVALIDCHAR:
                    return "Key name contains illegal char.";

                case WT_ERROR.WT_E_FUNCTION_NOTIMPLEMENTED:
                    return "Function not implemented.";

                case WT_ERROR.WT_E_TYPING_CSTYPEINVALID:
                    return "C# type not implemented.";
                case WT_ERROR.WT_E_TYPING_INITYPEINVALID:
                    return "INI type invalid {0}.";
                case WT_ERROR.WT_E_TYPING_INITYPEINOTIMPLEMENTED:
                    return "INI type not implemented {0}.";

                default:
                    return "<not_implemented>";
            }
        }

        public static void LogDebug(string message)
        {
        }

        public static void LogGenericError(Exception e,  string message = null)
        {
            LogGenericError(e);
        }

        public static void LogFault(WT_ERROR errorCode, string additionalInfo = null)
        {

        }
    }
}
