using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectburfSkill : ProjectSkill
    {
        public ProjectburfSkill(List<BurfStructure> burf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Burf = burf;
        }

        public double Damage
        {
            get; private set;
        }

        public List<BurfStructure> Burf
        {
            get; set;
        }
    }
}
