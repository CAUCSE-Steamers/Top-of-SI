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

        public LiveAndDieTogether() : base("동거동락", 3)
        {
            burfs = new List<IBurf>
            {
                new DamageSplashBurf(-0.2),
                new SocialityBurf(5)
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
            throw new NotImplementedException();
        }
    }
}
