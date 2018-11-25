using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class LiveAndDieTogether : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, -2)
        };

        public LiveAndDieTogether() : base("동고동락", 3)
        {
            burfs = new List<IBurf>
            {
                new HurtDamageBurf(-0.2) { RemainingTurn = int.MaxValue },
                new DamageSplashBurf(0.0) { RemainingTurn = int.MaxValue },
                new SocialityBurf(5) { RemainingTurn = int.MaxValue }
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
            foreach(var programmer in programmers)
            {
                foreach(var burf in burfs)
                {
                    programmer.RegisterBurf(burf);
                }
            }
        }
    }
}
