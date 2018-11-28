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

        public int MainStageLevel
        {
            get; set;
        }

        public ICollection<string> ClearedStageNames
        {
            get; private set;
        }

        public ICollection<ProgrammerSpec> ProgrammerSpecs
        {
            get; private set;
        }

        public Player()
        {
            Money = 50;
            MainStageLevel = 0;
            ClearedStageNames = new List<string>();

            ProgrammerSpecs = new List<ProgrammerSpec>();
        }

        public XElement ToXmlElement()
        {
            var playerRoot = new XElement("Player",
                                          new XAttribute("Money", Money),
                                          new XAttribute("MainStageLevel", MainStageLevel),
                                          ClearedStageNames.Select(name => new XElement("ClearedStage", new XAttribute("Name", name))),
                                          ProgrammerSpecs.Select(spec => spec.ToXmlElement()));

            return playerRoot;
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            Money = element.AttributeValue("Money", int.Parse);
            MainStageLevel = element.AttributeValue("MainStageLevel", int.Parse);

            var recoveredSpecs = new List<ProgrammerSpec>();
            foreach (var specElement in element.Elements("ProgrammerSpec"))
            {
                var spec = new ProgrammerSpec();
                spec.RecoverStateFromXml(specElement.ToString());
                recoveredSpecs.Add(spec);
            };

            ClearedStageNames.Clear();
            foreach (var stageElement in element.Elements("ClearedStage"))
            {
                ClearedStageNames.Add(stageElement.AttributeValue("Name"));
            }

            ProgrammerSpecs = recoveredSpecs;
        }
    }
}