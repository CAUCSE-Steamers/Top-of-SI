using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectSingleDeburfSkill : ProjectSkill
    {
        public ProjectSingleDeburfSkill(List<DeBurfStructure> deburf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Deburf = deburf;
        }

        public List<DeBurfStructure> Deburf
        {
            get; private set;
        }

        public override XElement ToXmlElement()
        {
            var baseElement = base.ToXmlElement();
            baseElement.Add(
                new XElement("Specialized",
                    Deburf.Select(deburf => deburf.ToXmlElement()))
            );

            return baseElement;
        }

        public override void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            var structures = new List<DeBurfStructure>();
            foreach (var element in rootElement.Elements("DeburfStructure"))
            {
                var deburf = new DeBurfStructure(DeburfType.None, 0, 0.0);
                deburf.RecoverStateFromXml(element.ToString());

                structures.Add(deburf);
            }

            Deburf = structures;
        }
    }
}
