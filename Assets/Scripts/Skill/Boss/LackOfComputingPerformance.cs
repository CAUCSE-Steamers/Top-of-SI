using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LackOfComputingPerformance : ProjectSkill, IMultiAttack
    {

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiAttack,
            Technique = RequiredTechType.Ai,
            Name = "LackOfComputingPerformance",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public LackOfComputingPerformance() : base(information, 20, 2)
        {

        }
    }
}
