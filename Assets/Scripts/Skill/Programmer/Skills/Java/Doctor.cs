using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Doctor : PassiveSkill, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "Doctor",
            Type = SkillType.Java,
            AcquisitionLevel = 0,
            MaximumLevel = 20,
            IconName = "Java",
            RequiredUpgradeCost = 4,
            DescriptionFunc = level => string.Format("JavaDoc에 익숙해져, 여러 패키지를 사용할 수 있습니다. 데미지가 {0}% 증가합니다.", level)
        };

        public Doctor() : base(information.Clone(), Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + (Information.AcquisitionLevel * 0.01));
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}