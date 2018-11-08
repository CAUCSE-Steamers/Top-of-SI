using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProgrammerStatus : IEventDisposable
    {
        public event Action<int> OnHealthChanged = delegate { };
        
        private int health, leadership = 0, sociality = 0;
        private double healRate, critical;

        private List<KeyValuePair<IBurf, int>> burfs = new List<KeyValuePair<IBurf, int>>();
        private List<DeBurfStructure> deburf = new List<DeBurfStructure>();

        public string PortraitName
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public Money Cost
        {
            get; set;
        }

        public ProgrammerStatus()
        {
            FullHealth = Health;
            AdditionalDamageRatio = 0.0;
            Cost = new Money(30, 300, 200);
            critical = 0.03;
            healRate = 0.1;
            leadership = 10;
            sociality = 10;
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

        public int Leadership
        {
            get
            {
                return leadership;
            }
            set
            {
                int var = value;
                if((int)(leadership / 10) < (int)((leadership + value) / 10))
                {
                    Health += 3;
                }
                leadership += var;
            }
        }

        public int Sociality
        {
            get
            {
                return sociality;
            }
            set
            {
                int var = value;
                if ((int)(sociality / 10) < (int)((sociality + value) / 10))
                {
                    HealRate += 0.03;
                }
                sociality += var;
            }
        }

        public double Critical
        {
            //TODO : when Attack, calculate rate and affect to damage.
            get; set;
        }

        public double HealRate
        {
            //TODO : When heal other programmer, get rate from this.
            // healing value = (object.FullHealth - object.Health) * healRate
            get; set;
        }

        public void AddBurf(IBurf newBurf, int persistentTurn)
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

        public IEnumerable<IBurf> DecayBurfAndFetchExpiredBurfs()
        {
            var expiredBurfs = burfs.Where(burfInformation => burfInformation.Value <= 0);

            burfs = burfs.Except(expiredBurfs)
                         .Select(burfInformation => new KeyValuePair<IBurf, int>(burfInformation.Key, burfInformation.Value - 1))
                         .ToList();

            return expiredBurfs.Select(burfInformation => burfInformation.Key);
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
