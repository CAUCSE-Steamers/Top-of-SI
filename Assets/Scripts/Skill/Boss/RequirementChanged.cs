using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class RequirementChanged : ProjectSkill, IMultiAttack
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Common,
            Name = "RequirementChanged",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public RequirementChanged() : base(information, 2.1, 3)
        {

        }
    }
}
