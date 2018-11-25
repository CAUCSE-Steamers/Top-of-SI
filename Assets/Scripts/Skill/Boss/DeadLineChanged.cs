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

        private static IEnumerable<IBurf> deburfs = new List<IBurf>
        {
            new CostIncrementBurf(0.25) { RemainingTurn = 3 },
            new aaa
        };

        private static List<DeBurfStructure> deburf = new List<DeBurfStructure>
        {
            new DeBurfStructure(DeburfType.ShortenDeadLine, Int32.MaxValue, 0.2),
            new DeBurfStructure(DeburfType.IncreaseMentalUsage, Int32.MaxValue, 0.25)
        };

        public DeadLineChanged() 
            : base(new List<IBurf>(deburfs.Select(deburf => deburf.Clone())), information, 3)
        {

        }
    }
}
