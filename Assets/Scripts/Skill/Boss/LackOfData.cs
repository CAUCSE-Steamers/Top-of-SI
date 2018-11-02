using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LackOfData : ProjectSkill, IMultiDeburf
    {
        public double DecreaseDamage = 0.5;
        public int DecreaseTurn = 2;
        public DeburfType deburfType = DeburfType.DecreaseDamage;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.DataAnalysis,
            Name = "LackOfData",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public LackOfData() : base(information, 0, 3)
        {

        }
    }
}
