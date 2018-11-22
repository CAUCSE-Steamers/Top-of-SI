using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProjectStatus : IXmlConvertible, IXmlStateRecoverable
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

        public XElement ToXmlElement()
        {
            return new XElement("ProjectStatus",
                    new XAttribute("Health", Health),
                    new XAttribute("FullHealth", FullHealth),
                    new XAttribute("Name", Name));
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);

            Health = rootElement.AttributeValue("Health", int.Parse);
            FullHealth = rootElement.AttributeValue("FullHealth", int.Parse);
            Name = rootElement.AttributeValue("Name");
        }
    }
}
