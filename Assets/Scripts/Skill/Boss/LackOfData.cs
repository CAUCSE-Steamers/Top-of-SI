using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LackOfData : ProjectMultiDeburfSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.DataAnalysis,
            Name = "LackOfData",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static List<DeBurfStructure> deburf = new List<DeBurfStructure>
        {
            new DeBurfStructure(DeburfType.DecreaseAttack, 2, 0.5)
        };

        public LackOfData() : base(deburf, information, 3)
        {

        }
    }
}
