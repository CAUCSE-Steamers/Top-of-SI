using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CounterEvolution : PassiveSkill, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "CounterEvolution",
            Type = SkillType.C,
            AcquisitionLevel = 0,
            MaximumLevel = 60,
            IconName = "C",
            RequiredUpgradeCost = 4,
            DescriptionFunc = level => string.Format("Low-Level C 언어를 배웁니다. 스킬 데미지가 {0}% 증가합니다.", level)
        };

        public CounterEvolution() : base(information.Clone(), Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedDamage(double baseDamage, ProjectType projectType, RequiredTechType techType)
        {
            return baseDamage * (1 + (Information.AcquisitionLevel * 0.01));
        }
    }
}
