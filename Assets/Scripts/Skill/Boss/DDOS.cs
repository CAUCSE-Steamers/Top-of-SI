using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DDOS : ProjectSkill, IBurf
    {
        public int turn = 2;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Network,
            Name = "DDOS",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public DDOS() : base(information, 0, 3)
        {

        }
    }
}