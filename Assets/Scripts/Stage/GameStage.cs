using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Model
{
    public class GameStage : IXmlConvertible, IXmlStateRecoverable
    {
        private List<IStageObjective> objectives;
        public GameStage()
        {
            objectives = new List<IStageObjective>();
            Boss = new ProjectSpec();
        }

        public string Title
        {
            get; set;
        }

        public int ElapsedDayLimit
        {
            get; set;
        }

        public string IconName
        {
            get; set;
        }

        public IEnumerable<IStageObjective> Objectives
        {
            get
            {
                return objectives;
            }
        }

        public bool MainStage
        {
            get; set;
        }

        public ProjectSpec Boss
        {
            get; set;
        }

        public IEnumerable<ProgrammerSpec> ProgrammerSpecs
        {
            get; set;
        }

        public int Reward
        {
            get; set;
        }

        public void AddObjectives(IEnumerable<IStageObjective> newObjectives)
        {
            objectives.AddRange(newObjectives);
        }

        public void RecoverStateFromXml(string rawXml)
        {
            var rootElement = XElement.Parse(rawXml);
            Title = rootElement.AttributeValue("Title");
            ElapsedDayLimit = rootElement.AttributeValue("TimeLimit", int.Parse);
            Reward = rootElement.AttributeValue("Reward", int.Parse);
            MainStage = rootElement.AttributeValue("MainStage", bool.Parse);
            IconName = rootElement.AttributeValue("Icon");

            Boss = new ProjectSpec();
            Boss.RecoverStateFromXml(rootElement.Element("ProjectSpec").ToString());
        }

        public XElement ToXmlElement()
        {
            // TODO: objectives?
            return new XElement("Stage",
                new XAttribute("Title", Title),
                new XAttribute("TimeLimit", ElapsedDayLimit),
                new XAttribute("Reward", Reward),
                new XAttribute("MainStage", MainStage),
                new XAttribute("Icon", IconName),
                Boss.ToXmlElement());
        }
    }
}
