using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class CVar : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "CVar",
            Type = SkillType.C,
            AcquisitionLevel = 0,
            MaximumLevel = 100,
            IconName = "C",
            RequiredUpgradeCost = 5,
            DescriptionFunc = level => string.Format("C 언어로 코드를 작성합니다. 프로젝트에게 {0}의 데미지를 입힙니다. (쿨타임 5턴, 명중률 90%)", level / 2)
        };

        public CVar()
            : base(information.Clone(), new List<PassiveSkill>() { new NoteDown(), new CounterEvolution() }, 5)
        {
            Accuracy = 0.9;
            Cost = 4;
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Sniper");
            }
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if(Information.AcquisitionLevel == 10)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("NoteDown")).ToArray()[0].EnableToLearn();
            }
            else if(Information.AcquisitionLevel == 40)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("CounterEvolution")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateSkillLevelDamage(double baseDamage)
        {
            return baseDamage * Information.AcquisitionLevel / 2;
        }
    }
}
