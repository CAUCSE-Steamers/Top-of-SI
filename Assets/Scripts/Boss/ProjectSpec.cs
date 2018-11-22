using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using Model;

public class ProjectSpec : IXmlConvertible, IXmlStateRecoverable
{
    public ProjectSpec()
    {
        Status = new ProjectStatus();
        Ability = new ProjectAbility(new List<ProjectSkill>(), ProjectType.None);
    }

    public ProjectStatus Status
    {
        get; set;
    }

    public ProjectAbility Ability
    {
        get; set;
    }

    public XElement ToXmlElement()
    {
        return new XElement("ProjectSpec", Status.ToXmlElement(),
                                          Ability.ToXmlElement());
    }

    public void RecoverStateFromXml(string rawXml)
    {
        var element = XElement.Parse(rawXml);

        Status.RecoverStateFromXml(element.Element("ProjectStatus").ToString());
        Ability.RecoverStateFromXml(element.Element("ProjectAbility").ToString());
    }
}
