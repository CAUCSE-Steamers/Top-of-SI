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
            : base(information, new List<PassiveSkill>() { new Overload() }, 4)
        {
            Accuracy = 0.9;
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if(Information.AcquisitionLevel == 5)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Overload")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel * 3;
        }
    }
}
