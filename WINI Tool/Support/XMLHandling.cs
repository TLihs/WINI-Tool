// WINI Tool
// Copyright (c) 2023 Toni Lihs
// Licensed under MIT License

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static WINI_Tool.Support.ExceptionHandling;

namespace WINI_Tool.Support
{
    public static class XMLHandling
    {
        public static T TryReadValue<T>(XElement parent, string elementName, T defaultValue)
        {
            if (defaultValue == null)
            {
                LogWarning("XMLHandling::TryReadValue<{0}>(..., ..., null) - Default value is null. Setting default value to default of type (= '{1}').",
                    typeof(T).Name, default(T).ToString());
                defaultValue = default(T);
            }

            if (string.IsNullOrWhiteSpace(elementName))
            {
                LogGenericError("XMLHandling::TryReadValue<{0}>(..., {1}, {2}) - Element name can't be null or whitespace.",
                    typeof(T).Name, string.IsNullOrEmpty(elementName) ? "null" : "whitespace", defaultValue.ToString());
                return defaultValue;
            }

            if (parent == null)
            {
                LogGenericError("XMLHandling::TryReadValue<{0}>(null, {1}, {2}) - Parent element is null.",
                    typeof(T).Name, elementName, defaultValue.ToString());
                return defaultValue;
            }

            XElement element = parent.Element(elementName);
            
            if (element == null)
            {
                LogWarning("XMLHandling::TryReadValue<{0}>({1}, {2}, {3}) - No child element with name '{2}' found.",
                    typeof(T).Name, parent.Name.ToString(), elementName, defaultValue.ToString());
                return defaultValue;
            }

            string elementvalue = element.Value;

            try
            {
                T returnvalue = (T)Convert.ChangeType(elementvalue, typeof(T));
                return returnvalue;
            }
            catch (Exception ex)
            {
                LogGenericError(ex);
                return defaultValue;
            }
        }

        public static List<T> TryReadListValue<T>(XElement superParent, string parentName, string elementName, T defaultValue)
        {
            if (defaultValue == null)
            {
                LogWarning("XMLHandling::TryReadListValue<{0}>(..., ..., ..., null) - Default value is null. Setting default value to default of type (= '{1}').",
                    typeof(T).Name, default(T).ToString());
                defaultValue = default(T);
            }

            List<T> returnlist = new List<T>();

            if (string.IsNullOrWhiteSpace(parentName))
            {
                LogGenericError("XMLHandling::TryReadListValue<{0}>(..., {1}, ..., {2}) - Parent element name can't be null or whitespace.",
                    typeof(T).Name, string.IsNullOrEmpty(parentName) ? "null" : "whitespace", defaultValue.ToString());
                return returnlist;
            }

            if (superParent == null)
            {
                LogGenericError("XMLHandling::TryReadListValue<{0}>(null, {1}, {2}, {3}) - Super parent element is null.",
                    typeof(T).Name, parentName, elementName, defaultValue.ToString());
                return returnlist;
            }

            XElement parentelement = superParent.Element(elementName);

            if (parentelement == null)
            {
                LogWarning("XMLHandling::TryReadListValue<{0}>({1}, {2}, {3}, {4}) - No child element with name '{2}' found.",
                    typeof(T).Name, parentelement.Name.ToString(), elementName, defaultValue.ToString());
                return returnlist;
            }

            foreach (XElement element in parentelement.Elements())
                if (element != null && element.Name == elementName)
                    returnlist.Add(TryReadValue(element, elementName, defaultValue));

            return returnlist;
        }
    }
}
