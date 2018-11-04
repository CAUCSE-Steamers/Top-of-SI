using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace Model.Formation
{
    public abstract class Formation
    {
        public readonly static IEnumerable<Formation> formations;
        
        static Formation()
        {
            var formationType = typeof(Formation);

            formations = from type in formationType.Assembly.GetTypes()
                         where type.Namespace == formationType.Namespace &&
                               type.IsSubclassOf(formationType)
                         let constructor = type.GetConstructor(new Type[] { })
                         select constructor.Invoke(new object[] { }) as Formation;
        }

        public Formation(string name, int numberOfProgrammers)
        {
            Name = name;
            NumberOfProgrammers = numberOfProgrammers;
        }

        public string Name
        {
            get; private set;
        }

        public int NumberOfProgrammers
        {
            get; private set;
        }

        public void Attach(IEnumerable<Programmer> programmers)
        {

        }

        public void Detach()
        {

        }

        public bool CanApplyFormation()
        {
            var programmersInStage = StageManager.Instance.Unit.Programmers;

            if (programmersInStage.Count() != NumberOfProgrammers)
            {
                return false;
            }

            return programmersInStage.Any(programmer => CanApplyFormationCentralFor(programmer));
        }

        private bool CanApplyFormationCentralFor(Programmer programmer)
        {
            var stageField = StageManager.Instance.StageField;
            var programmers = StageManager.Instance.Unit.Programmers;
            var centralLocation = stageField.VectorToIndices(programmer.transform.position);

            var relativePositions = new List<Vector2Int>
            {
                Vector2Int.zero
            };

            foreach (var otherProgrammer in programmers)
            {
                var relativeIndices = centralLocation - stageField.VectorToIndices(otherProgrammer.transform.position);

                if (relativeIndices != Vector2Int.zero)
                {
                    relativePositions.Add(relativeIndices);
                }
            }

            return relativePositions.Distinct()
                                    .All(position => RelativeFormation.Contains(position));
        }

        protected abstract IEnumerable<Vector2Int> RelativeFormation
        {
            get;
        }

        protected abstract IEnumerable<BurfStructure> FormationsBurfs
        {
            get;
        }
    }
}
