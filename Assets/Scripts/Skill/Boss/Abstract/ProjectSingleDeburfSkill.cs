using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectSingleDeburfSkill : ProjectSkill
    {
        public ProjectSingleDeburfSkill(List<DeBurfStructure> deburf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Deburf = deburf;
        }

        public double Damage
        {
            get; private set;
        }

        public List<DeBurfStructure> Deburf
        {
            get; private set;
        }
    }
}
