using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class OutOfOnePixel : ProjectSingleAttackSkill, ISoundProducible
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.SingleAttack,
            Technique = RequiredTechType.Ui,
            Name = "OutOfOnePixel",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public OutOfOnePixel() : base(30, information, 2)
        {

        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Stab");
            }
        }
    }
}
