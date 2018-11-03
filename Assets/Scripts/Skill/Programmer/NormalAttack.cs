using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class NormalAttack : ActiveSkill
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "NormalAttack",
            Type = SkillType.None,
            AcquisitionLevel = 1,
            MaximumLevel = 1,
            IconName = "Cpp",
            RequiredUpgradeCost = 0
        };

        public NormalAttack() 
            : base(information, Enumerable.Empty<PassiveSkill>(), 33.0, 3.0)
        {
            Accuracy = 1.0;
        }

        //TODO: TechniqueType도 추가할 것.

        protected override double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            return BaseDamage;
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel;
        }
    }
}
