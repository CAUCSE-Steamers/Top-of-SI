using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class DeadLineChanged : ProjectMultiDeburfSkill, ISoundProducible
    {
        private const int defaultCooldown = 3;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.Common, 
            Name = "DeadLineChanged", 
            MaximumLevel = 1, 
            Animation = "Shout"
        };

        private static IEnumerable<IBurf> deburfs = new List<IBurf>
        {
            new CostIncrementBurf(0.25) { RemainingTurn = defaultCooldown },
            new MaximumLimitChangedBurf(-0.2) { RemainingTurn = defaultCooldown }
        };

        public DeadLineChanged() 
            : base(new List<IBurf>(deburfs.Select(deburf => deburf.Clone())), information, defaultCooldown)
        {

        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Roar");
            }
        }
    }
}
