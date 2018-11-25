using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class Agile : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1)
        };

        public Agile() : base("애자일", 4)
        {
            burfs = new List<IBurf>
            {
                new HurtDamageBurf(-0.25) { RemainingTurn = int.MaxValue },
                new DamageSplashBurf(0.0) { RemainingTurn = int.MaxValue },
                new SocialityBurf(8) { RemainingTurn = int.MaxValue }
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
                foreach (var burf in burfs)
                {
                    programmer.RegisterBurf(burf);
                }
            }
        }
    }
}
