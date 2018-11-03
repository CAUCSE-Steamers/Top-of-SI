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
