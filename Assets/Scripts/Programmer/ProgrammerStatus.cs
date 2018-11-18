using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProgrammerStatus : IEventDisposable, IXmlConvertible, IXmlStateRecoverable
    {
        public event Action<int> OnHealthChanged = delegate { };
        
        private int health;

        private List<IBurf> burfs = new List<IBurf>();
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
            HealRate = 0.0;
            Leadership = 10;
            Sociality = 10;
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

        public void AddLeadership(int quantity)
        {
            if ((int)(Leadership / 10) < (int)((Leadership + quantity) / 10))
            {
                Health += 3;
            }

            Leadership += quantity;
        }

        public int Leadership
        {
            get; private set;
        }

        public void AddSociality(int quantity)
        {
            if ((int)(Sociality / 10) < (int)((Sociality + quantity) / 10))
            {
                HealRate += 0.03;
            }
        }

        public int Sociality
        {
            get; private set;
        }

        public double HealRate
        {
            get; set;
        }

        public void AddBurf(IBurf newBurf)
        {
            burfs.Add(newBurf);
        }

        public void RemoveBurf(IBurf targetBurf)
        {
            burfs = burfs.Where(burf => burf.Equals(targetBurf) == false)
                         .ToList();
        }

        public IEnumerable<IBurf> Burfs
        {
            get
            {
                return burfs;
            }
        }

        public IEnumerable<IBurf> PositiveBurfs
        {
            get
            {
                return Burfs.Where(burf => burf.IsPositiveBurf);
            }
        }

        public IEnumerable<IBurf> NegativeBurfs
        {
            get
            {
                return Burfs.Where(burf => burf.IsPositiveBurf == false);
            }
        }

        public IEnumerable<IBurf> DecayBurfAndFetchExpiredBurfs()
        {
            var expiredBurfs = burfs.Where(burf => burf.RemainingTurn <= 0);

            burfs = burfs.Except(expiredBurfs)
                         .ToList();
            burfs.ForEach(burf => --burf.RemainingTurn);

            return expiredBurfs;
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

        public XElement ToXmlElement()
        {
            return new XElement("Status",
                    new XAttribute("Portrait", PortraitName),
                    new XAttribute("Name", Name),
                    new XAttribute("FullHealth", FullHealth),
                    new XAttribute("Health", Health),
                    new XAttribute("Leadership", Leadership),
                    new XAttribute("Sociality", Sociality),
                    new XAttribute("HealRate", HealRate), 
                    Cost.ToXmlElement());
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            PortraitName = element.AttributeValue("Portrait");
            Name = element.AttributeValue("Name");
            FullHealth = element.AttributeValue("FullHealth", int.Parse);
            Health = element.AttributeValue("Health", int.Parse);
            Leadership = element.AttributeValue("Leadership", int.Parse);
            Sociality = element.AttributeValue("Sociality", int.Parse);
            HealRate = element.AttributeValue("HealRate", double.Parse);

            Cost.RecoverStateFromXml(element.Element("Money").ToString());
        }
    }
}
