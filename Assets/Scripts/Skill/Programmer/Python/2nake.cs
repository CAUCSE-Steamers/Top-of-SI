using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class snake : ActiveSkill
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "2nake",
            Type = SkillType.Python,
            AcquisitionLevel = 1,
            MaximumLevel = 10,
            IconName = "Python",
            RequiredUpgradeCost = 1
        };

        public snake()
            : base(information, Enumerable.Empty<PassiveSkill>(), 1, 1)
        {
            Accuracy = 0.8;
        }

        protected override double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            //TODO: calculate Synastry.
            return BaseDamage;
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel;
        }
    }
}
