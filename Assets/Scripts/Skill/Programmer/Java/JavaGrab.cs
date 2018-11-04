using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class JavaGrab : ActiveSkill
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "JavaGrab",
            Type = SkillType.Java,
            AcquisitionLevel = 1,
            MaximumLevel = 10,
            IconName = "Java",
            RequiredUpgradeCost = 3
        };

        public JavaGrab()
            : base(information, Enumerable.Empty<PassiveSkill>(), 1, 2)
        {
            Accuracy = 0.9;
            //TODO: Add Auxilirary Passive Skill
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 3)
            {
                //TODO: Enable Doctor
            }
            else if (Information.AcquisitionLevel == 7)
            {
                //TODO: Enable MachineLearning
            }
        }

        protected override double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            //TODO: calculate Synastry.
            return BaseDamage;
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel * 2;
        }
    }
}
