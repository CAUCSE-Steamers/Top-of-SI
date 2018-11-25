using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectSingleDeburfSkill : ProjectSkill
    {
        public ProjectSingleDeburfSkill(IEnumerable<IBurf> programmerBurfs, ProjectSkillInformation information, double defaultCooldown) : base(information, defaultCooldown)
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
