using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OSUpdate : ProjectSkill, IBurf
    {
        public double decreaseDamage = 0.666666;
        public int decreaseTurn = 2;
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Desktop, 
            Name = "OSUpdate", 
            MaximumLevel = 1, 
            Animation = "Shout"
        };

        public OSUpdate() : base(information, 0, 3)
        {

        }
    }
}
