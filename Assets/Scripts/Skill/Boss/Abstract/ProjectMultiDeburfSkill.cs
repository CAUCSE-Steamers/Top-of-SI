using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectMultiDeburfSkill : ProjectSkill
    {
        public ProjectMultiDeburfSkill(List<DeBurfStructure> deburf, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Deburf = deburf;
        }

        public List<DeBurfStructure> Deburf
        {
            get; private set;
        }
    }
}
