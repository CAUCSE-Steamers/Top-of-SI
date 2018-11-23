using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DeadLineChanged : ProjectMultiDeburfSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.Common, 
            Name = "DeadLineChanged", 
            MaximumLevel = 1, 
            Animation = "Shout"
        };

        private static List<DeBurfStructure> deburf = new List<DeBurfStructure>
        {
            new DeBurfStructure(DeburfType.ShortenDeadLine, Int32.MaxValue, 0.2),
            new DeBurfStructure(DeburfType.IncreaseMentalUsage, Int32.MaxValue, 0.25)
        };

        public DeadLineChanged() : base(deburf, information, 3)
        {

        }
    }
}
