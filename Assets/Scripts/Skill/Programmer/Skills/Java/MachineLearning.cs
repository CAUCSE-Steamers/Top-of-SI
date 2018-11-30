using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MachineLearning : PassiveSkill, ICooldownConvertible, IAccuracyConvertible, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "MachineLearning",
            Type = SkillType.Java,
            AcquisitionLevel = 0,
            MaximumLevel = 30,
            IconName = "Java",
            RequiredUpgradeCost = 4,
            DescriptionFunc = level => string.Format("JVM을 공부합니다. 쿨타임이 {0}% 증가하지만, 명중률과 데미지가 {0}% 증가합니다.", level * 2, level)
        };

        public MachineLearning() : base(information.Clone(), Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedAccuracy(double accuracy)
        {
            return accuracy * (1 + (Information.AcquisitionLevel * 0.01));
        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 + (2 * Information.AcquisitionLevel * 0.01));
        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + (Information.AcquisitionLevel * 0.01));
        }
    }
}
