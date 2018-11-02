using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DeadLineChanged : ProjectSkill, IMultiDeburf
    {
        public double shortenDeadline = 0.8;
        public double IncreaseMentalUsage = 0.75;
        public DeburfType deburfType = DeburfType.IncreaseMentalUsage & DeburfType.ShortenDeadLine;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.Common, 
            Name = "DeadLineChanged", 
            MaximumLevel = 1, 
            Animation = "Shout"
        };

        public DeadLineChanged() : base(information, 0, 3)
        {

        }
    }
}
