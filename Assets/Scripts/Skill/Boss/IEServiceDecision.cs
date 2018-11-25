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

        private static IEnumerable<IBurf> deburfs = new List<IBurf>
        {
            new CostIncrementBurf(1.0) { RemainingTurn = 4 },
        };

        public IEServiceDecision() 
            : base(new List<IBurf>(deburfs.Select(deburf => deburf.Clone())), information, 3)
        {

        }
    }
}