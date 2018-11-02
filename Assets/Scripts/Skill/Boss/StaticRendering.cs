using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class StaticRendering : ProjectSkill, ISoloDeburf
    {
        public DeburfType deburftype = DeburfType.DisableMovement;
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.SingleDeburf,
            Technique = RequiredTechType.Graphic,
            Name = "StaticRendering",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public StaticRendering() : base(information, 0, 4)
        {

        }
    }
}
