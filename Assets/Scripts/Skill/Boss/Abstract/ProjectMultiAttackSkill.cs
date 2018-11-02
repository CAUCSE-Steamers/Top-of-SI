using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectMultiAttackSkill : ProjectSkill
    {
        public ProjectMultiAttackSkill(double damage, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Damage = damage;
        }

        public double Damage
        {
            get; private set;
        }
    }
}
