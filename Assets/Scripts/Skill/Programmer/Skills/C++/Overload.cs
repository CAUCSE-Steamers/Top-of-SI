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
            RequiredUpgradeCost = 8,
            DescriptionFunc = level => string.Format("연산자 오버로딩을 시작합니다. 스킬의 쿨타임과 데미지가 {0}% 증가합니다.", level * 5)
        };

        public Overload() : base(information.Clone(), Enumerable.Empty<PassiveSkill>())
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
