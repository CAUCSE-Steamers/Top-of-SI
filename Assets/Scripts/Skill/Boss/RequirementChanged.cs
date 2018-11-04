using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    class RequirementChanged : ProjectBurfSkill, ISoundProducible
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Common,
            Name = "RequirementChanged",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static List<BurfStructure> burf = new List<BurfStructure>
        {
            new BurfStructure(BurfType.Cure, 0, 0.1)
        };

        public RequirementChanged() : base(burf, information, 3)
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
