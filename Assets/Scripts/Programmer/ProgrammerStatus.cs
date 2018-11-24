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

        public void ResetStageParameters()
        {
            StartVacationDay = null;
            AdditionalDamageRatio = 0.0;
            HealRate = 0.0;
            RemainingVacationDay = 0;
            burfs.Clear();
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
            int currentLeadershipBonusLevel = Leadership / 10;
            int updatedLeadershipBonusLevel = (Leadership + quantity) / 10;
            int levelDifference = updatedLeadershipBonusLevel - currentLeadershipBonusLevel;

            if (levelDifference > 0)
            {
                CommonLogger.LogFormat("ProgrammerStatus::AddLeadership => 리더쉽 증가 효과로 정신력/총 정신력이 {0}만큼 증가함.", 3 * levelDifference);

                FullHealth += 3 * levelDifference;
                Health += 3 * levelDifference;
            }

            Leadership += quantity;
        }

        public int Leadership
        {
            get; private set;
        }

        public void AddSociality(int quantity)
        {
            int currentSocialityBonusLevel = Sociality / 10;
            int updatedSocialityBonusLevel = (Sociality + quantity) / 10;
            int levelDifference = updatedSocialityBonusLevel - currentSocialityBonusLevel;

            if (levelDifference > 0)
            {
                CommonLogger.LogFormat("ProgrammerStatus::AddSociality => 사교성 증가 효과로 회복량이 {0}만큼 증가함.", 0.03 * levelDifference);
                HealRate += 0.03 * levelDifference;
            }

            Sociality += quantity;
        }

        public int Sociality
        {
            get; private set;
        }

        public double HealRate
        {
            get; set;
        }

        public int RemainingVacationDay
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

        public bool isValidBurfs(String Name)
        {
            foreach(var iter in Burfs.ToList())
            {
                if(iter.IconName.Equals(Name))
                {
                    return true;
                }
            }
            return false;
        }



        public IEnumerable<IBurf> DecayBurfAndFetchExpiredBurfs()
        {
            burfs.ForEach(burf => --burf.RemainingTurn);

            var expiredBurfs = burfs.Where(burf => burf.RemainingTurn <= 0);

            burfs = burfs.Where(burf => burf.RemainingTurn > 0)
                         .ToList();

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
