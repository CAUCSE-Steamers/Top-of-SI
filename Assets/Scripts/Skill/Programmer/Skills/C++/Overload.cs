using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Overload : PassiveSkill, ICooldownConvertible, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "Overload",
            Type = SkillType.CPlusPlus,
            AcquisitionLevel = 0,
            MaximumLevel = 10,
            IconName = "CPlusPlus",
            RequiredUpgradeCost = 8
        };

        public Overload() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 + 5 * information.AcquisitionLevel / 100);
        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + 5 * information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
