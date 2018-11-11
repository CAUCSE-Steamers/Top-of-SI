using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public static class XmlUtility
    {
        public static string AttributeValue(this XElement element, string attributeName)
        {
            return element.Attribute(attributeName).Value;
        }

        public static T AttributeValue<T>(this XElement element, string attributeName, Func<string, T> convertFunction)
        {
            return convertFunction(element.AttributeValue(attributeName));
        }
    }
}
