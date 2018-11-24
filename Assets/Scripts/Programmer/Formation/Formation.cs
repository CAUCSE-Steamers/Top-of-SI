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
        protected static Programmer central;
        protected IEnumerable<IBurf> burfs;
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

        public IEnumerable<Programmer> AffectedProgrammers
        {
            get; protected set;
        }

        public void AttachBurfs(IEnumerable<Programmer> programmers)
        {
            AffectedProgrammers = programmers;
            RegisterBurfs(programmers);
        }

        protected abstract void RegisterBurfs(IEnumerable<Programmer> programmers);

        public virtual void DetachBurfs()
        {
            foreach (var programmer in AffectedProgrammers.ToList())
            {
                foreach (var burf in burfs)
                {
                    programmer.Status.RemoveBurf(burf);
                }
            }
        }

        public bool CanApplyFormation()
        {
            var programmersInStage = StageManager.Instance.Unit.NotVacationProgrammers;

            if (programmersInStage.Count() != NumberOfProgrammers)
            {
                return false;
            }

            return programmersInStage.Any(programmer => CanApplyFormationCentralFor(programmer));
        }

        private bool CanApplyFormationCentralFor(Programmer programmer)
        {
            var stageField = StageManager.Instance.StageField;
            var centralLocation = stageField.VectorToIndices(programmer.transform.position);
            var relativePositions = FetchRelativePositionsFor(centralLocation);
            var isValid = relativePositions.Distinct()
                                    .All(position => RelativeFormation.Contains(position));
            if (isValid)
            {
                central = programmer;
            }
            return isValid;
        }

        private IEnumerable<Vector2Int> FetchRelativePositionsFor(Vector2Int centralLocation)
        {
            var stageField = StageManager.Instance.StageField;
            var programmers = StageManager.Instance.Unit.NotVacationProgrammers;

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

            return relativePositions;
        }

        protected abstract IEnumerable<Vector2Int> RelativeFormation
        {
            get;
        }
    }
}
