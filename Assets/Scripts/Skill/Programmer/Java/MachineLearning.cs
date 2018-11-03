using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class MachineLearning : PassiveSkill, ICooldownConvertible, IAccuracyConvertible, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "MachineLearning",
            Type = SkillType.Java,
            AcquisitionLevel = 0,
            MaximumLevel = 30,
            IconName = "Java",
            RequiredUpgradeCost = 4
        };

        public MachineLearning() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double calculateAppliedAccuracy(double accuracy)
        {
            return accuracy * (1 + Information.AcquisitionLevel / 100);
        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 + 2 * information.AcquisitionLevel / 100);
        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + Information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
