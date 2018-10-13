using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProgrammerStatus
    {
        public ProgrammerStatus()
        {
            FullHealth = Health;
        }

        public int? StartVacationDay
        {
            get; set;
        }

        public int FullHealth
        {
            get; set;
        }

        public int Health
        {
            get; set;
        }
    }
}
