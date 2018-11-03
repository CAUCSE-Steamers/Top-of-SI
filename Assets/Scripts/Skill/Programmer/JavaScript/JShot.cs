using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class JShot : ActiveSkill
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "JShot",
            Type = SkillType.JavaScript,
            AcquisitionLevel = 1,
            MaximumLevel = 30,
            IconName = "JavaScript",
            RequiredUpgradeCost = 2
        };

        public JShot()
            : base(information, Enumerable.Empty<PassiveSkill>(), 1, 2)
        {
            Accuracy = 0.5;
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
