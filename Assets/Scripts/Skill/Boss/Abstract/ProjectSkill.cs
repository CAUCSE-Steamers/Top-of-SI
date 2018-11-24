using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public abstract class ProjectSkill : ICooldownRequired, IXmlConvertible, IXmlStateRecoverable
    {
        private double cooldown;

        public static ProjectSkill ParseXml(XElement element)
        {
            var skillType = typeof(ProjectSkill).Assembly
                                                .GetTypes()
                                                .Where(type => type.FullName == element.AttributeValue("Type"))
                                                .Single();

            var recoveredSkill = skillType.GetConstructor(new Type[] { })
                                          .Invoke(new object[] { }) as ProjectSkill;

            recoveredSkill.Information.RecoverStateFromXml(element.Element("ProjSkillInfo").ToString());
            recoveredSkill.RecoverStateFromXml(element.Element("Specialized").ToString());

            return recoveredSkill;
        }

        public ProjectSkill(ProjectSkillInformation information, double defaultCooldown)
        {
            Information = information;
            DefaultCooldown = defaultCooldown;

            cooldown = 0;
        }

        public bool IsTriggerable
        {
            get
            {
                return cooldown < double.Epsilon;
            }
        }

        public void DecreaseCooldown()
        {
            cooldown -= 1.0;
        }

        public void ForceCooldownToAvailable()
        {
            cooldown = 0.0;
        }

        public void ResetCoolDown()
        {
            cooldown = DefaultCooldown;
        }

        public virtual XElement ToXmlElement()
        {
            return new XElement("ProjectSkill",
                new XAttribute("Type", GetType().FullName),
                new XAttribute("Cooldown", DefaultCooldown),
                Information.ToXmlElement());
        }

        public abstract void RecoverStateFromXml(string rawXml);

        public ProjectSkillInformation Information
        {
            get; private set;
        }

        public double DefaultCooldown
        {
            get; private set;
        }

        public double Priority
        {
            get
            {
                return DefaultCooldown - cooldown;
            }
        }
        
    }
}
