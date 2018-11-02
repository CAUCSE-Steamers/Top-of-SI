using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectSingleAttackSkill : ProjectSkill
    {
        public ProjectSingleAttackSkill(double damage, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Damage = damage;
        }

        public double Damage
        {
            get; private set;
        }
    }
}
