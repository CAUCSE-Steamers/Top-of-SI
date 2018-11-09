using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class NoteDown : PassiveSkill, ICooldownConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "NoteDown",
            Type = SkillType.C,
            AcquisitionLevel = 0,
            MaximumLevel = 40,
            IconName = "C",
            RequiredUpgradeCost = 4
        };

        public NoteDown() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedCooldown(double baseCooldown)
        {
            return baseCooldown * (1 - information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
