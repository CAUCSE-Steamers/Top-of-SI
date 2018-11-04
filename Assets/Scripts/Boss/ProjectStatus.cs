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
        private List<BurfStructure> burf = new List<BurfStructure>();

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

        public List<BurfStructure> Burf
        {
            get
            {
                return burf;
            }
            set
            {
                burf.AddRange(value);
            }
        }

        public bool OnBurf(BurfType status)
        {
            foreach (var iter in burf)
            {
                if ((iter.Type & status) > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
