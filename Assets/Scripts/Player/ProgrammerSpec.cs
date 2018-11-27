using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class ProgrammerSpec : IXmlConvertible, IXmlStateRecoverable
    {
        public ProgrammerAbility Ability
        {
            get; private set;
        }

        public ProgrammerStatus Status
        {
            get; set;
        }

        public ProgrammerSpec()
        {
            Ability = new ProgrammerAbility();
            Status = new ProgrammerStatus
            {
                PortraitName = "UnityChan",
                FullHealth = 50,
                Health = 50,
                Name = "테스트 보스"
            };
        }

        public XElement ToXmlElement()
        {
            return new XElement("ProgrammerSpec", Status.ToXmlElement(),
                                              Ability.ToXmlElement());
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var element = XElement.Parse(rawXml);

            Status.RecoverStateFromXml(element.Element("Status").ToString());
            Ability.RecoverStateFromXml(element.Element("Ability").ToString());
        }
    }
}
