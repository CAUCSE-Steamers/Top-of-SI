using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class DDOS : ProjectBurfSkill, ISoundProducible
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.Burf,
            Technique = RequiredTechType.Network,
            Name = "DDOS",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        private static List<BurfStructure> burf = new List<BurfStructure>
        {
            new BurfStructure(BurfType.Overwhelming, 2, 0)
        };

        public DDOS() : base(burf, information, 3)
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