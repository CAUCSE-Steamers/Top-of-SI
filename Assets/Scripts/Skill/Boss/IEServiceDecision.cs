using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class IEServiceDecision : ProjectMultiDeburfSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.Web,
            Name = "IEServiceDecision",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static List<DeBurfStructure> deburf = new List<DeBurfStructure>
        {
            new DeBurfStructure(DeburfType.IncreaseMentalUsage, Int32.MaxValue, 1)
        };

        public IEServiceDecision() : base(deburf, information, 3)
        {

        }
    }
}