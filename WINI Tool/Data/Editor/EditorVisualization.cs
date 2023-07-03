// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

using static WINI_Tool.Support.ExceptionHandling;
using static WINI_Tool.Support.Constants;

namespace WINI_Tool.Data.Editor
{
    public static class EditorVisualization
    {
        public enum MARKING : uint
        {
            INFO_KEY = 0x_0000_0001,
            INFO_GROUP = 0x_0000_0002,
            INFO_SECTION = 0x_0000_0003,
            INFO_COMMENTKEY = 0x_0000_0004,
            INFO_COMMENTGROUP = 0x_0000_0005,
            INFO_COMMENTSECTION = 0x_0000_0006,

            INFO_VALUE_STRING = 0x_0000_0010,
            INFO_VALUE_INTEGER = 0x_0000_0011,
            INFO_VALUE_FLOAT = 0x_0000_0012,
            INFO_VALUE_BOOL = 0x_0000_0013,
            INFO_VALUE_DATE = 0x_0000_0014,
            INFO_VALUE_PATH = 0x_0000_0015,

            INFO_NO_MARKING = 0x_1000_0000,

            WRN_KEY_FORMAT = 0x_3000_0001,
            WRN_GROUP_FORMAT = 0x_3000_0002,
            WRN_SECTION_FORMAT = 0x_3000_0003,
            WRN_COMMENT_FORMAT = 0x_3000_0004,

            WRN_VALUE_ENCODING = 0x_5000_0001,
            WRN_KEY_ENCODING = 0x_5000_0002,
            WRN_GROUP_ENCODING = 0x_5000_0003,
            WRN_SECTION_ENCODING = 0x_5000_0004,
            WRN_COMMENT_ENCODING = 0x_5000_0005,

            ERR_LINECONTENT_NOMATCH = 0x_8000_0001,
            ERR_SECTION_NOMATCH = 0x_8000_0002,
            ERR_GROUP_NOMATCH = 0x_8000_0003,
            ERR_KEY_NOMATCH = 0x_8000_0004,

            ERR_SECTION_INVALIDCHAR = 0x_8000_0010,
            ERR_GROUP_INVALIDCHAR = 0x_8000_0011,
            ERR_KEY_INVALIDCHAR = 0x_8000_0012,
        }

        public static Dictionary<Regex, RXColorization> RegisteredFormats = new Dictionary<Regex, RXColorization>();

        static EditorVisualization()
        {
            RXColorization.CreateAndRegister(RXKeyValuePair, MARKING.INFO_NO_MARKING);
        }

        public static string MarkingToString(MARKING mark)
        {
            switch (mark)
            {
                case MARKING.ERR_LINECONTENT_NOMATCH:
                    return "Line content does not match any regular expression.";

                case MARKING.ERR_SECTION_INVALIDCHAR:
                    return "Section name contains illegal char.";

                case MARKING.ERR_GROUP_INVALIDCHAR:
                    return "Group name contains illegal char.";

                case MARKING.ERR_KEY_INVALIDCHAR:
                    return "Key name contains illegal char.";

                default:
                    return string.Format("<{0}>", Enum.GetName(typeof(MARKING), mark));
            }
        }

        public static RXColorization MarkingToColorization(MARKING mark)
        {
            Log(EXCEPTIONTYPES.ERR_FUNCTION_NOTIMPLEMENTED, "RXColorization::MarkingToColorization(..)");
            return null;
            
            //switch (mark)
            //{
            //    case MARKING.ERR_LINECONTENT_NOMATCH:
            //        return "Line content does not match any regular expression.";

            //    case MARKING.ERR_SECTION_INVALIDCHAR:
            //        return "Section name contains illegal char.";

            //    case MARKING.ERR_GROUP_INVALIDCHAR:
            //        return "Group name contains illegal char.";

            //    case MARKING.ERR_KEY_INVALIDCHAR:
            //        return "Key name contains illegal char.";

            //    case MARKING.ERR_TYPING_CSTYPEINVALID:
            //        return "C# type not implemented.";

            //    case MARKING.ERR_TYPING_INITYPEINVALID:
            //        return "INI type invalid {0}.";

            //    case MARKING.ERR_TYPING_INITYPEINOTIMPLEMENTED:
            //        return "INI type not implemented {0}.";

            //    default:
            //        return string.Format("<{0}>", Enum.GetName(typeof(MARKING), mark));
            //}
        }

        public class RXColorization
        {
            public MARKING Marking { get; set; }
            public Regex Format { get; }
            public Color Background { get; set; }
            public Color Foreground { get; set; }
            public Color Border { get; set; }
            public Color SelectedBackground { get; set; }
            public Color SelectedForeground { get; set; }
            public Color SelectedBorder { get; set; }

            private RXColorization(Regex format, MARKING marking)
            {
                Format = format;
                Marking = marking;

                Background = Colors.Transparent;
                Foreground = Colors.Gray;
                Border = Colors.Transparent;
                SelectedBackground = Colors.Transparent;
                SelectedForeground = Colors.DarkGray;
                SelectedBorder = Colors.Black;
            }

            public static RXColorization CreateAndRegister(Regex format, MARKING marking)
            {
                if (format == null)
                {
                    LogGenericError(new ArgumentNullException("RXColorization::Create(..) - Format cannot be null."));
                    return null;
                }

                if (RegisteredFormats.ContainsKey(format))
                {
                    LogWarning(string.Format("RXColorization::Create({0}) - Format already registered.", format.ToString()));
                    return RegisteredFormats[format];
                }

                RXColorization newcolorization = new RXColorization(format, marking);
                RegisteredFormats.Add(format, newcolorization);
                return newcolorization;
            }
        }
    }
}
