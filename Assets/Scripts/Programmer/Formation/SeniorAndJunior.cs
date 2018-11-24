using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class SeniorAndJunior : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0)
        };

        public SeniorAndJunior() : base("사수제", 2)
        {
            burfs = new List<IBurf>
            {
                new DamageSpreadBurf(0.5, 0) { RemainingTurn = int.MaxValue },
                new LeadershipBurf(3) { RemainingTurn = int.MaxValue }
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
                    programmer.RegisterBurf(burfs.ElementAt(1));
                }
                else
                {
                    programmer.RegisterBurf(burfs.ElementAt(0));
                }
            }
        }
    }
}
