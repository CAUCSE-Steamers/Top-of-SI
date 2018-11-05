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

        public ProgrammerStatus()
        {
            FullHealth = Health;
            AdditionalDamageRatio = 0.0;


            RegisterBurf(new HealBurf(10, 50), 3);
            RegisterBurf(new HurtDamageBurf(10.0), 2);
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

            var statusModificationCommand = newBurf as IStatusModificationCommand;
            if (statusModificationCommand != null && newBurf.IsPersistent == false)
            {
                statusModificationCommand.Modify(this);
            }
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
            var expiredBurfs = burfs.Where(burfInformation => burfInformation.Value <= 1);

            foreach (var expireBurf in expiredBurfs.Select(burfInformation => burfInformation.Key)
                                                   .OfType<IStatusModificationCommand>())
            {
                expireBurf.Unmodify(this);
            }

            burfs = burfs.Except(expiredBurfs)
                         .Select(burfInformation => new KeyValuePair<IBurf, int>(burfInformation.Key, burfInformation.Value - 1))
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
