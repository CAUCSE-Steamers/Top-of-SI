using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class FastCode : PassiveSkill, ICooldownConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "FastCode",
            Type = SkillType.Python,
            AcquisitionLevel = 0,
            MaximumLevel = 10,
            IconName = "Python",
            RequiredUpgradeCost = 2
        };

        public FastCode() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 - information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
