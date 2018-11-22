using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectSingleAttackSkill : ProjectSkill
    {
        public ProjectSingleAttackSkill(double damage, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Damage = damage;
        }

        public double Damage
        {
            get; private set;
        }

        public override XElement ToXmlElement()
        {
            var baseElement = base.ToXmlElement();
            baseElement.Add(
                new XElement("Specialized",
                    new XAttribute("Damage", Damage))
            );

            return baseElement;
        }

        public override void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);
            Damage = rootElement.AttributeValue("Damage", double.Parse);
        }
    }
}
