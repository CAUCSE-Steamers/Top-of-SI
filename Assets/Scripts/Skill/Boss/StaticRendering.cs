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

        private static List<DeBurfStructure> deburf = new List<DeBurfStructure>
        {
            new DeBurfStructure(DeburfType.DisableMovement, Int32.MaxValue, 0)
        };

        public StaticRendering() : base(deburf, information, 4)
        {

        }
    }
}
