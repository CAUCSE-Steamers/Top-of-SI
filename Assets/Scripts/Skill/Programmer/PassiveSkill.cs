using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public abstract class PassiveSkill : ILevelUp, IXmlConvertible
    {
        public static PassiveSkill ParseXml(XElement skillElement)
        {
            var skillType = typeof(ActiveSkill).Assembly
                                               .GetTypes()
                                               .Where(type => type.FullName == skillElement.AttributeValue("Type"))
                                               .Single();

            var recoveredSkill = skillType.GetConstructor(new Type[] { })
                                         .Invoke(new object[] { }) as PassiveSkill;

            recoveredSkill.Information.RecoverStateFromXml(skillElement.Element("SkillInfo").ToString());

            var passiveSkills = new List<PassiveSkill>();
            foreach (var passiveElement in skillElement.Element("Auxiliary").Elements())
            {
                passiveSkills.Add(ParseXml(passiveElement));
            }

            recoveredSkill.AuxiliaryPassiveSkills = passiveSkills;
            return recoveredSkill;
        }

        public PassiveSkill(SkillBasicInformation information, IEnumerable<PassiveSkill> auxiliaryPassiveSkills)
        {
            Information = information;
            AuxiliaryPassiveSkills = auxiliaryPassiveSkills;
        }

        public SkillBasicInformation Information
        {
            get; private set;
        }

        public void EnableToLearn()
        {
            Information.LearnEnabled = true;
        }

        public IEnumerable<PassiveSkill> AuxiliaryPassiveSkills
        {
            get; private set;
        }

        public abstract void LevelUP();

        public XElement ToXmlElement()
        {
            var passiveRoot = new XElement("Passive",
                new XAttribute("Type", GetType().FullName),
                Information.ToXmlElement());

            passiveRoot.Add(new XElement("Auxiliary",
                AuxiliaryPassiveSkills.Select(passiveSkill => passiveSkill.ToXmlElement())));

            return passiveRoot;
        }
    }
}
