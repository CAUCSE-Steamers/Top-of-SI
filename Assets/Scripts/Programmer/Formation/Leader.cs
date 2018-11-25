using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class Leader : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, 0),
            new Vector2Int(-2, -1)
        };

        public Leader() : base("리더", 4)
        {
            burfs = new List<IBurf>
            {
                new TargetedDamageSharingBurf(0.33) { RemainingTurn = int.MaxValue },
                new LeadershipBurf(5) { RemainingTurn = int.MaxValue },
                new HealBurf(5, 5) { RemainingTurn = int.MaxValue }
            };
        }

        protected override IEnumerable<Vector2Int> RelativeFormation
        {
            get
            {
                return relativeFormationLocations;
            }
        }

        protected override void RegisterBurfs(IEnumerable<Programmer> programmers)
        {
            foreach (var programmer in programmers)
            {
                if (programmer.Equals(CenterProgrammer))
                {
                    (burfs.ElementAt(0) as TargetedDamageSharingBurf).TargetProgrammer = programmer;

                    programmer.RegisterBurf(burfs.ElementAt(1));
                    programmer.RegisterBurf(burfs.ElementAt(2));
                }
                else
                {
                    programmer.RegisterBurf(burfs.ElementAt(0));
                }
            }
        }
    }
}
