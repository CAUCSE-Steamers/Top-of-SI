using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class VersionUpdate : ProjectMultiAttackSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiAttack,
            Technique = RequiredTechType.Common,
            Name = "VersionUpdate",
            MaximumLevel = 1,
            Animation = "Attack"
        };

        public VersionUpdate() : base(10.0, information, 0)
        {

        }
    }
}
