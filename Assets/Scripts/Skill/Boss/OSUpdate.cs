using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class OSUpdate : ProjectBurfSkill, ISoundProducible
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Desktop, 
            Name = "OSUpdate", 
            MaximumLevel = 1, 
            Animation = "Shout"
        };

        private static List<BurfStructure> burf = new List<BurfStructure>
        {
            new BurfStructure(BurfType.DecreaseDamage, 2, 0.33333)
        };

        public OSUpdate() : base(burf, information, 3)
        {

        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("monster2");
            }
        }
    }
}
