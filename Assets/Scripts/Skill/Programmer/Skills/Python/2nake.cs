using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class Snake : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "2nake",
            Type = SkillType.Python,
            AcquisitionLevel = 0,
            MaximumLevel = 10,
            IconName = "Python",
            RequiredUpgradeCost = 1,
            DescriptionFunc = level => string.Format("Python으로 코드를 작성합니다. 프로젝트에게 {0}의 데미지를 입힙니다. (쿨타임 1턴, 명중률 80%)", level)
        };

        public Snake()
            : base(information.Clone(), new List<PassiveSkill>() { new FastCode(), new Pypi() }, 1)
        {
            Accuracy = 0.8;
            Cost = 2;
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
            if (Information.AcquisitionLevel == 5)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("FastCode")).ToArray()[0].EnableToLearn();
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Pypi")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double baseDamage)
        {
            return baseDamage * Information.AcquisitionLevel;
        }
    }
}
