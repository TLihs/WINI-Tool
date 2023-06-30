// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Diagnostics;
using System.IO;

using static WPFExceptionHandler.ExceptionManagement;

namespace WINI_Tool.Support
{
    public static class ExceptionHandling
    {
        public enum EXCEPTIONTYPES : uint
        {
            ERR_FUNCTION_NOTIMPLEMENTED = 0x_8000_0000,

            ERR_EH_FILELOGGINGDEACTIVATED = 0x_8000_0011,

            ERR_TYPING_CSTYPEINVALID = 0x_8000_0020,
            ERR_TYPING_INITYPEINVALID = 0x_8000_0021,
            ERR_TYPING_INITYPEINOTIMPLEMENTED = 0x_8000_0022,
        }

        public static bool UseFileLogging => EHUseFileLogging;
        public static string LogFilePath
        {
            get
            {
                if (!EHUseFileLogging)
                    Log(EXCEPTIONTYPES.ERR_EH_FILELOGGINGDEACTIVATED, string.Empty);
                return EHUseFileLogging ? EHExceptionLogFilePath : string.Empty;
            }
            set
            {
                if (!EHUseFileLogging)
                    Log(EXCEPTIONTYPES.ERR_EH_FILELOGGINGDEACTIVATED, string.Empty);
                else
                    SetAlternativeLogFilePath(value);
            }
        }

        static ExceptionHandling()
        {

        }

        public static void Create(bool useFileLogging, string logPath = "")
        {
            Debug.Print("Initializing ExceptionHandling...");
            CreateExceptionManagement(App.Current, AppDomain.CurrentDomain, true, useFileLogging);
            if (!string.IsNullOrEmpty(logPath))
                SetAlternativeLogFilePath(logPath);
            Debug.Print("ExceptionHandling initialized.");
        }

        public static string ExceptionToString(EXCEPTIONTYPES exceptionType)
        {
            switch (exceptionType)
            {
                case EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED:
                    return "Function not implemented.";

                case EXCEPTIONTYPES.ERR_EH_FILELOGGINGDEACTIVATED:
                    return "Logging to file not active.";

                default:
                    return string.Format("<{0}>", Enum.GetName(typeof(EXCEPTIONTYPES), exceptionType));
            }
        }

        public static void Log(EXCEPTIONTYPES type, string message)
        {
            EHLogGenericError("{0} ({1})", ExceptionToString(type), message);
        }

        public static void Log(LogEntryType type, string message)
        {
            switch (type)
            {
                case LogEntryType.LE_WARNING:
                    EHLogWarning(message);
                    break;

                case LogEntryType.LE_ERROR_GENERIC:
                    EHLogGenericError(message);
                    break;

                case LogEntryType.LE_ERROR_CRITICAL:
                    EHLogCriticalError(message);
                    break;

                default:
                    EHLogDebug(message);
                    break;
            }
        }

        public static void LogDebug(string message, params string[] formatParameters) => EHLogDebug(message, formatParameters);
        public static void LogWarning(string message, params string[] formatParameters) => EHLogWarning(message, formatParameters);
        public static void LogGenericError(string message, params string[] formatParameters) => EHLogGenericError(message, formatParameters);
        public static void LogGenericError(Exception exception) => EHLogGenericError(exception);
        public static void LogCriticalError(Exception exception) => EHLogCriticalError(exception);
    }
}
