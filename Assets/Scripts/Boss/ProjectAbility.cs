using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectAbility
    {
        public ProjectAbility(List<ProjectSkill> projectSkills, ProjectType projectType)
        {
            ProjectSkills = projectSkills;
            foreach (ProjectSkill iter in ProjectSkills)
            {
                Techtype = Techtype | iter.Information.Technique;
            }
            ProjType = ProjType;
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

        public ProjectSkill InvokedSkill
        {
            get
            {
                ProjectSkills = ProjectSkills.OrderByDescending(ProjectSkill => ProjectSkill.Priority);
                return ProjectSkills.First();
            }
        }
    }
}
