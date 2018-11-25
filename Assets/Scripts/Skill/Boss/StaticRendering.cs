using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class StaticRendering : ProjectSingleDeburfSkill
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.SingleDeburf,
            Technique = RequiredTechType.Graphic,
            Name = "StaticRendering",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        // TODO: Block burf
        private static IEnumerable<IBurf> deburfs = new List<IBurf>
        {
            new MaximumLimitChangedBurf(-0.2) { RemainingTurn = 2 }
        };

        public StaticRendering() 
            : base(new List<IBurf>(deburfs.Select(deburf => deburf.Clone())), information, 4)
        {

        }
    }
}
