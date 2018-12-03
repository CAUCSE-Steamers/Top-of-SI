using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class JShot : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "JShot",
            Type = SkillType.JavaScript,
            AcquisitionLevel = 0,
            MaximumLevel = 30,
            IconName = "JavaScript",
            RequiredUpgradeCost = 2,
            DescriptionFunc = level => string.Format("JavaScript로 코드를 작성합니다. 프로젝트에게 {0}의 데미지를 입힙니다. (쿨타임 2턴, 명중률 50%)", level)
        };

        public JShot()
            : base(information.Clone(), new List<PassiveSkill>() { new ReadableScript() }, 2)
        {
            Accuracy = 0.5;
            Cost = 2;
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Cannon");
            }
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 15)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("ReadableScript")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double baseDamage)
        {
            return baseDamage * Information.AcquisitionLevel;
        }
    }
}
