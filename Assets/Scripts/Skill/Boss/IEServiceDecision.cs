using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class IEServiceDecision : ProjectSkill, IMultiDeburf
    {
        public double IncreaseMentalUsage = 2;
        public DeburfType deburfType = DeburfType.IncreaseMentalUsage;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.Web,
            Name = "IEServiceDecision",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public IEServiceDecision() : base(information, 0, 3)
        {

        }
    }
}