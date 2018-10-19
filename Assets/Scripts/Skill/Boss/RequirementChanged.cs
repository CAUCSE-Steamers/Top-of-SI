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
            Technique = TechniqueType.Common,
            Name = "RequirementChanged",
            MaximumLevel = 1
        };

        public RequirementChanged() : base(information, 0.1, 3)
        {

        }
    }
}
