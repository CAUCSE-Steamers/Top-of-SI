using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class C8 : ActiveSkill
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "C8",
            Type = SkillType.CPlusPlus,
            AcquisitionLevel = 1,
            MaximumLevel = 20,
            IconName = "Cpp",
            RequiredUpgradeCost = 5
        };

        public C8()
            : base(information, Enumerable.Empty<PassiveSkill>(), 1, 4)
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
            return projectTypeAppliedDamage * Information.AcquisitionLevel * 3;
        }
    }
}
