using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectAbility : IXmlConvertible, IXmlStateRecoverable
    {
        public ProjectAbility(List<ProjectSkill> projectSkills, ProjectType projectType)
        {
            ProjectSkills = projectSkills;
            foreach (ProjectSkill iter in ProjectSkills)
            {
                Techtype = Techtype | iter.Information.Technique;
            }

            ProjType = projectType;
        }

        public IEnumerable<ProjectSkill> ProjectSkills
        {
            get; private set;
        }

        public RequiredTechType Techtype
        {
            get; private set;
        }

        public ProjectType ProjType
        {
            get; private set;
        }

        public ProjectAbility Clone()
        {
            foreach (var skill in ProjectSkills)
            {
                skill.ForceCooldownToAvailable();
            }

            return new ProjectAbility(new List<ProjectSkill>(this.ProjectSkills), this.ProjType)
            {
                Techtype = this.Techtype,
            };
        }

        public ProjectSkill InvokedSkill
        {
            get
            {
                ProjectSkills = ProjectSkills.OrderByDescending(ProjectSkill => ProjectSkill.Priority);
                foreach (ProjectSkill iter in ProjectSkills)
                {
                    if (iter.IsTriggerable)
                    {
                        return iter;
                    }
                }
                return ProjectSkills.First();
            }
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            ProjType = (ProjectType) rootElement.AttributeValue("ProjectType", int.Parse);
            Techtype = (RequiredTechType) rootElement.AttributeValue("TechType", int.Parse);

            var skillList = new List<ProjectSkill>();
            foreach (var skillElement in rootElement.Element("Skills").Elements())
            {
                skillList.Add(ProjectSkill.ParseXml(skillElement));
            }

            ProjectSkills = skillList;
        }

        public XElement ToXmlElement()
        {
            return new XElement("ProjectAbility",
                new XAttribute("ProjectType", (int) ProjType),
                new XAttribute("TechType", (int) Techtype),
                new XElement("Skills", ProjectSkills.Select(skill => skill.ToXmlElement())));
        }
    }
}
