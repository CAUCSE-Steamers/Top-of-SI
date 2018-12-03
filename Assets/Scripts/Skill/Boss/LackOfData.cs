using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class LackOfData : ProjectMultiDeburfSkill, ISoundProducible
    {
        private const int defaultCooldown = 3;

        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.MultiDeburf,
            Technique = RequiredTechType.DataAnalysis,
            Name = "LackOfData",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static IEnumerable<IBurf> deburfs = new List<IBurf>
        {
            new SkillDamageBurf(-0.5) { RemainingTurn = defaultCooldown }
        };

        public LackOfData() 
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
