using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProgrammerAbility
    {
        public ProgrammerAbility()
        {
            AcquiredActiveSkills = new List<ActiveSkill>
            {
                new NormalAttack()
            };   
        }

        public IEnumerable<ActiveSkill> AcquiredActiveSkills
        {
            get; private set;
        }

        public int MentalPower
        {
            get; set;
        }
    }
}
