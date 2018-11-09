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
            : base(information, new List<PassiveSkill>() { new FastCode(), new Pypi() }, 1)
        {
            Accuracy = 0.8;
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 5)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("FastCode")).ToArray()[0].EnableToLearn();
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Pypi")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel;
        }
    }
}
