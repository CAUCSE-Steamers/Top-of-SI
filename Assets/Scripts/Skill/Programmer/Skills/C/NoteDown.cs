using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class NoteDown : PassiveSkill, ICooldownConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "NoteDown",
            Type = SkillType.C,
            AcquisitionLevel = 0,
            MaximumLevel = 40,
            IconName = "C",
            RequiredUpgradeCost = 4,
            DescriptionFunc = level => string.Format("C 언어로 개발한 코드를 다른 곳에 적용할 수 있습니다. C 언어 공격의 쿨타임이 {0}% 감소합니다.", level)
        };

        public NoteDown() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 - (information.AcquisitionLevel * 0.01));
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
