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
            RequiredUpgradeCost = 2,
            DescriptionFunc = level => string.Format("Python 라이브러리를 사용하기 시작합니다. 데미지가 {0}% 증가하지만, 쿨타임이 {1}% 증가합니다.", level, level * 5)
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
