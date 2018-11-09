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
            RequiredUpgradeCost = 3
        };

        public CVar()
            : base(information, Enumerable.Empty<PassiveSkill>(), 1, 5)
        {
            Accuracy = 0.9;
            //TODO: Add Auxilirary Passive Skill
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("Cannon");
            }
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if(Information.AcquisitionLevel == 10)
            {
                //TODO: Enable NoteDown
            }
            else if(Information.AcquisitionLevel == 40)
            {
                //TODO: Enable ConterEvolution.
            }
        }

        protected override double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            //TODO: calculate Synastry.
            return BaseDamage;
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel / 2;
        }
    }
}
