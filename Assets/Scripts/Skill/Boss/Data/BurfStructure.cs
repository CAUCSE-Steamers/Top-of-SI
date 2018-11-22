using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class BurfStructure : IXmlConvertible, IXmlStateRecoverable
    {
        public BurfStructure(BurfType type, int turn, double factor)
        {
            Type = type;
            Turn = turn;
            Factor = factor;
        }
        public BurfType Type
        {
            get; private set;
        }

        public int Turn
        {
            get; private set;
        }

        public double Factor
        {
            get; private set;
        }

        public void DecreaseTurn()
        {
            Turn--;
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            Type = (BurfType) rootElement.AttributeValue("Type", int.Parse);
            Turn = rootElement.AttributeValue("Turn", int.Parse);
            Factor = rootElement.AttributeValue("Factor", double.Parse);
        }

        public XElement ToXmlElement()
        {
            return new XElement("BurfStructure",
                new XAttribute("Type", (int) Type),
                new XAttribute("Turn", Turn),
                new XAttribute("Factor", Factor));
        }
    }
}
