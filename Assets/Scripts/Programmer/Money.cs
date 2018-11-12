using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class Money : IXmlConvertible, IXmlStateRecoverable
    {
        public Money(int hire, int pay, int fire)
        {
            Hire = hire;
            Pay = pay;
            Fire = fire;
        }
        public int Hire
        {
            get; private set;
        }
        public int Pay
        {
            get; private set;
        }
        public int Fire
        {
            get; private set;
        }

        public XElement ToXmlElement()
        {
            return new XElement("Money",
                new XAttribute("Hire", Hire),
                new XAttribute("Pay", Pay),
                new XAttribute("Fire", Fire));
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            Hire = element.AttributeValue("Hire", int.Parse);
            Pay = element.AttributeValue("Pay", int.Parse);
            Fire = element.AttributeValue("Fire", int.Parse);
        }
    }
}
