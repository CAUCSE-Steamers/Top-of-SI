using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class LackOfComputingPerformance : ProjectMultiAttackSkill, ISoundProducible
    {

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiAttack,
            Technique = RequiredTechType.Ai,
            Name = "LackOfComputingPerformance",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public LackOfComputingPerformance() : base(20, information, 2)
        {

        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("DropSword");
            }
        }
    }
}
