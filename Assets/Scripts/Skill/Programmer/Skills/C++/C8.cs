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
            AcquisitionLevel = 0,
            MaximumLevel = 20,
            IconName = "Cpp",
            RequiredUpgradeCost = 5,
            DescriptionFunc = level => string.Format("C++로 코드를 작성합니다. 프로젝트에게 {0}의 데미지를 입힙니다. (쿨타임 4턴, 명중률 90%)", level * 3)
        };

        public C8()
            : base(information.Clone(), new List<PassiveSkill>() { new Overload() }, 4)
        {
            Accuracy = 0.9;
            Cost = 4;
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if(Information.AcquisitionLevel == 5)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Overload")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double baseDamage)
        {
            return baseDamage * Information.AcquisitionLevel * 3;
        }
    }
}
