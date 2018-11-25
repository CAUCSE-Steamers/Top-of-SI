using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectMultiDeburfSkill : ProjectSkill
    {
        public ProjectMultiDeburfSkill(ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
        }

        public ProjectMultiDeburfSkill(IEnumerable<IBurf> programmerBurfs, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
        {
            Deburf = programmerBurfs;
        }

        public IEnumerable<IBurf> Deburf
        {
            get; private set;
        }

        public override void RecoverStateFromXml(string rawXml)
        {

        }
    }
}
