using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectBurfSkill : ProjectSkill
    {
        public ProjectBurfSkill(List<BurfStructure> burf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Burf = burf;
        }

        public List<BurfStructure> Burf
        {
            get; set;
        }

        public override XElement ToXmlElement()
        {
            var baseElement = base.ToXmlElement();
            baseElement.Add(
                new XElement("Specialized",
                    Burf.Select(burf => burf.ToXmlElement()))
            );

            return baseElement;
        }

        public override void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            var structures = new List<BurfStructure>();
            foreach (var element in rootElement.Elements("BurfStructure"))
            {
                var deburf = new BurfStructure(BurfType.None, 0, 0.0);
                deburf.RecoverStateFromXml(element.ToString());

                structures.Add(deburf);
            }

            Burf = structures;
        }
    }
}
