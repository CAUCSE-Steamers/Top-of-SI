using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class JavaGrab : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "JavaGrab",
            Type = SkillType.Java,
            AcquisitionLevel = 0,
            MaximumLevel = 10,
            IconName = "Java",
            RequiredUpgradeCost = 3,
            DescriptionFunc = level => string.Format("Java로 코드를 작성합니다. 프로젝트에게 {0}의 데미지를 입힙니다. (쿨타임 2턴, 명중률 90%)", level * 2)
        };

        public JavaGrab()
            : base(information, new List<PassiveSkill>() { new Doctor(), new MachineLearning() }, 2)
        {
            Accuracy = 0.9;
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("SilenceGun");
            }
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 3)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Doctor")).ToArray()[0].EnableToLearn();
            }
            else if (Information.AcquisitionLevel == 7)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("MachineLearning")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel * 2;
        }
    }
}
