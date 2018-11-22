using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectSkillInformation : IXmlConvertible, IXmlStateRecoverable
    {
        public ProjectSkillType Type
        {
            get; set;
        }

        public RequiredTechType Technique
        {
            get; set;
        }

        public String Name
        {
            get; set;
        }

        public int MaximumLevel
        {
            get; set;
        }

        public String Animation
        {
            get; set;
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            Type = (ProjectSkillType) rootElement.AttributeValue("SkillType", int.Parse);
            Technique = (RequiredTechType) rootElement.AttributeValue("TechType", int.Parse);
            MaximumLevel = rootElement.AttributeValue("MaxLevel", int.Parse);
            Animation = rootElement.AttributeValue("Animation");
        }

        public XElement ToXmlElement()
        {
            return new XElement("ProjSkillInfo",
                new XAttribute("SkillType", (int) Type),
                new XAttribute("TechType", (int) Technique),
                new XAttribute("MaxLevel", MaximumLevel),
                new XAttribute("Animation", Animation));
        }
    }
}
