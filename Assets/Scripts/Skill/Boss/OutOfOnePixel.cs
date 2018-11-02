using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OutOfOnePixel : ProjectSkill, ISoloAttack
    {
        private static ProjectSkillInformation information = new ProjectSkillInformation
        {
            Type = ProjectSkillType.SingleAttack,
            Technique = RequiredTechType.Ui,
            Name = "OutOfOnePixel",
            MaximumLevel = 1,
            Animation = "Shout"
        };

        public OutOfOnePixel() : base(information, 30, 2)
        {

        }
    }
}
