using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Model
{
    public class Player : IXmlConvertible, IXmlStateRecoverable
    {
        public int Money { get; set; }
        public ICollection<ProgrammerSpec> ProgrammerSpecs
        {
            get; private set;
        }

        public Player()
        {
            this.Money = 1000;
            ProgrammerSpecs = new List<ProgrammerSpec>
            {
                new ProgrammerSpec(),
                new ProgrammerSpec(),
                new ProgrammerSpec()
            };
        }

        public XElement ToXmlElement()
        {
            var playerRoot = new XElement("Player",
                                          new XAttribute("Money", Money),
                                          ProgrammerSpecs.Select(spec => spec.ToXmlElement()));

            return playerRoot;
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            Money = element.AttributeValue("Money", int.Parse);

            var recoveredSpecs = new List<ProgrammerSpec>();
            foreach (var specElement in element.Elements("ProgrammerSpec"))
            {
                var spec = new ProgrammerSpec();
                spec.RecoverStateFromXml(specElement.ToString());
                recoveredSpecs.Add(spec);
            };

            ProgrammerSpecs = recoveredSpecs;
        }
    }
}