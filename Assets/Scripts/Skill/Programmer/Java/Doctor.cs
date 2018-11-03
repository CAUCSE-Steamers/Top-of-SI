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
            RequiredUpgradeCost = 4
        };

        public Doctor() : base(information, Enumerable.Empty<PassiveSkill>())
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