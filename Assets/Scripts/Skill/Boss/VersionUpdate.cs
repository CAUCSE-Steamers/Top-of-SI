using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class VersionUpdate : ProjectSkill, IMultiAttack
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiAttack,
            Technique = TechniqueType.Common,
            Name = "VersionUpdate",
            MaximumLevel = 1
        };

        public VersionUpdate() : base(information, 10.0, 0)
        {

        }
    }
}
