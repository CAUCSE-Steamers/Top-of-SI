using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class RequirementChanged : ProjectBurfSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Common,
            Name = "RequirementChanged",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static List<BurfStructure> burf = new List<BurfStructure>
        {
            new BurfStructure(BurfType.Cure, 0, 0.1)
        };

        public RequirementChanged() : base(burf, information, 3)
        {

        }
    }
}
