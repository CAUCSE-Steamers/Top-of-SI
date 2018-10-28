using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GameStage
    {
        private List<IStageObjective> objectives;

        public GameStage()
        {
            objectives = new List<IStageObjective>();
        }

        public string Title
        {
            get; set;
        }

        public int ElapsedDayLimit
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

        public void AddObjectives(IEnumerable<IStageObjective> newObjectives)
        {
            objectives.AddRange(newObjectives);
        }
    }
}
