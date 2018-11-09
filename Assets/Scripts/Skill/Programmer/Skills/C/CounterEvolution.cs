using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class CounterEvolution : PassiveSkill, IDamageConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "CounterEvolution",
            Type = SkillType.C,
            AcquisitionLevel = 0,
            MaximumLevel = 60,
            IconName = "C",
            RequiredUpgradeCost = 4
        };

        public CounterEvolution() : base(information, Enumerable.Empty<PassiveSkill>())
        {

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
