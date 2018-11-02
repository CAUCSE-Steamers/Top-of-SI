using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectBurfSkill : ProjectSkill
    {
        public ProjectBurfSkill(List<BurfStructure> burf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Burf = burf;
        }

        public List<BurfStructure> Burf
        {
            get; set;
        }
    }
}
