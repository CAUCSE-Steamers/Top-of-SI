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

        private List<KeyValuePair<IBurf, int>> burfs = new List<KeyValuePair<IBurf, int>>
        {
            new KeyValuePair<IBurf, int>(new HealBurf(100, 100), 2)
        };
        private List<DeBurfStructure> deburf = new List<DeBurfStructure>();

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
            AdditionalDamageRatio = 0.0;
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

        public double AdditionalDamageRatio
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

        public void RegisterBurf(IBurf newBurf, int persistentTurn)
        {
            burfs.Add(new KeyValuePair<IBurf, int>(newBurf, persistentTurn));
        }

        public void RemoveBurf(IBurf targetBurf)
        {
            burfs = burfs.Where(burfInformation => burfInformation.Key.Equals(targetBurf) != false)
                         .ToList();
        }

        public IEnumerable<IBurf> Burfs
        {
            get
            {
                return burfs.Select(burfInformation => burfInformation.Key);
            }
        }

        public void DecayBurfs()
        {
            burfs = burfs.Select(burfInformation => new KeyValuePair<IBurf, int>(burfInformation.Key, burfInformation.Value - 1))
                         .Where(burfInformation => burfInformation.Value > 0)
                         .ToList();
        }

        public List<DeBurfStructure> Deburf
        {
            get
            {
                return deburf;
            }
            set
            {
                deburf.AddRange(value);
            }
        }

        public bool OnDeburf(DeburfType status)
        {
            foreach (var iter in deburf)
            {
                if ((iter.Type & status) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void DisposeRegisteredEvents()
        {
            OnHealthChanged = delegate { };
        }
    }
}
