using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public abstract class PassiveSkill : ILevelUp
    {
        private bool enableToLearn = false;
        public PassiveSkill(SkillBasicInformation information, IEnumerable<PassiveSkill> auxiliaryPassiveSkills)
        {
            Information = information;
            AuxiliaryPassiveSkills = auxiliaryPassiveSkills;
        }

        public SkillBasicInformation Information
        {
            get; private set;
        }

        public void EnableToLearn()
        {
            enableToLearn = true;
        }

        public IEnumerable<PassiveSkill> AuxiliaryPassiveSkills
        {
            get; private set;
        }

        public abstract void LevelUP();
    }
}
