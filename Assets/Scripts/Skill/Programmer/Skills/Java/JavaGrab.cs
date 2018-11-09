using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class JavaGrab : ActiveSkill, ISoundProducible
    {
        private static SkillBasicInformation information = new SkillBasicInformation
        {
            Name = "JavaGrab",
            Type = SkillType.Java,
            AcquisitionLevel = 1,
            MaximumLevel = 10,
            IconName = "Java",
            RequiredUpgradeCost = 3
        };

        public JavaGrab()
            : base(information, new List<PassiveSkill>() { new Doctor(), new MachineLearning() }, 2)
        {
            Accuracy = 0.9;
        }

        public AudioClip EffectSound
        {
            get
            {
                return ResourceLoadUtility.LoadEffectClip("SilenceGun");
            }
        }

        public override void LevelUP()
        {
            Information.AcquisitionLevel++;
            if (Information.AcquisitionLevel == 3)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("Doctor")).ToArray()[0].EnableToLearn();
            }
            else if (Information.AcquisitionLevel == 7)
            {
                FlattenContainingPassiveSkills().Where(x => x.Information.Name.Equals("MachineLearning")).ToArray()[0].EnableToLearn();
            }
        }

        protected override double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            //TODO: calculate Synastry.
            return BaseDamage;
        }

        protected override double CalculateSkillLevelDamage(double projectTypeAppliedDamage)
        {
            return projectTypeAppliedDamage * Information.AcquisitionLevel * 2;
        }
    }
}
