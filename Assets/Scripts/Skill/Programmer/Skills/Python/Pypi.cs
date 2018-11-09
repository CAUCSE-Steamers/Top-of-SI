using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Pypi : PassiveSkill, ICooldownConvertible, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "Pypi",
            Type = SkillType.Python,
            AcquisitionLevel = 0,
            MaximumLevel = 20,
            IconName = "Python",
            RequiredUpgradeCost = 2
        };

        public Pypi() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 + 5 * information.AcquisitionLevel / 100);
        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
