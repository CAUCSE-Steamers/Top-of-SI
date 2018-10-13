using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public abstract class PassiveSkill
    {
        public PassiveSkill(SkillBasicInformation information, IEnumerable<PassiveSkill> auxiliaryPassiveSkills)
        {
            Information = information;
            AuxiliaryPassiveSkills = auxiliaryPassiveSkills;
        }

        public SkillBasicInformation Information
        {
            get; private set;
        }

        public IEnumerable<PassiveSkill> AuxiliaryPassiveSkills
        {
            get; private set;
        }
    }
}
