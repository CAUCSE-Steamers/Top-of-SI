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
            AcquisitionLevel = 0,
            MaximumLevel = 30,
            IconName = "JavaScript",
            RequiredUpgradeCost = 2
        };

        public JShot()
            : base(information, new List<PassiveSkill>() { new ReadableScript() }, 2)
        {
            Accuracy = 0.5;
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 15)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("ReadableScript")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel;
        }
    }
}
