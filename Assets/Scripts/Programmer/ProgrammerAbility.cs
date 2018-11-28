using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProgrammerAbility : IXmlConvertible, IXmlStateRecoverable
    {
        private ICollection<ActiveSkill> acquiredActiveSkills;

        public ProgrammerAbility()
        {
            acquiredActiveSkills = new List<ActiveSkill>
            {
                new NormalAttack(),
                new JavaGrab(),
                new CVar(),
                new C8(),
                new JShot(),
                new Snake(),
            };   
        }

        public IEnumerable<ActiveSkill> AcquiredActiveSkills
        {
            get
            {
                return acquiredActiveSkills;
            }
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            acquiredActiveSkills.Clear();

            foreach (var skillElement in element.Element("AcquiredSkills").Elements())
            {
                acquiredActiveSkills.Add(ActiveSkill.ParseXml(skillElement));
            }
        }

        public XElement ToXmlElement()
        {
            var abilityRoot = new XElement("Ability");

            abilityRoot.Add(new XElement("AcquiredSkills",
                AcquiredActiveSkills.Select(acquiredSkill => acquiredSkill.ToXmlElement())));

            return abilityRoot;
        }
    }
}
