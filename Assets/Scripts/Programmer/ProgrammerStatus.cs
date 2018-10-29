using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProgrammerStatus : IEventDisposable
    {
        public event Action<int> OnHealthChanged = delegate { };

        private int health;

        public string PortraitName
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public ProgrammerStatus()
        {
            FullHealth = Health;
        }

        public int? StartVacationDay
        {
            get; set;
        }

        public bool IsOnVacation
        {
            get
            {
                return StartVacationDay != null;
            }
        }

        public int FullHealth
        {
            get; set;
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                OnHealthChanged(health);
            }
        }

        public void DisposeRegisteredEvents()
        {
            OnHealthChanged = delegate { };
        }
    }
}
