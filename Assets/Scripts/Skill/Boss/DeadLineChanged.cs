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
            new CostIncrementBurf(0.25) { RemainingTurn = 4 },
            new MaximumLimitChangedBurf(-0.2) { RemainingTurn = 1 }
        };

        public DeadLineChanged() 
            : base(new List<IBurf>(deburfs.Select(deburf => deburf.Clone())), information, 3)
        {

        }
    }
}
