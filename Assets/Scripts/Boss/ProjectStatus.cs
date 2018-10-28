using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectStatus
    {
        public event Action<int> OnHealthChanged = delegate { };
        private int health;

        public ProjectStatus()
        {
            FullHealth = Health;
        }

        public string Name
        {
            get; set;
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
    }
}
