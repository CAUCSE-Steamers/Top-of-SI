using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class NormalAttack : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "NormalAttack",
            Type = SkillType.None,
            AcquisitionLevel = 1,
            MaximumLevel = 1,
            IconName = "Typing",
            RequiredUpgradeCost = 0
        };

        public NormalAttack() 
            : base(information, Enumerable.Empty<PassiveSkill>(), 1.0)
        {
            Accuracy = 1.0;
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Laser");
            }
        }

        public override void LevelUP()
        {
            // Do nothing
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage;
        }
    }
}
