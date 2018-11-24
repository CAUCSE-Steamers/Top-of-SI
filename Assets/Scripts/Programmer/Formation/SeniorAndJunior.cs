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
                new DamageSpreadBurf(0.5, 0),
                new LeadershipBurf(3)
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
            foreach(var iter in programmers.ToList())
            {
                if(iter == central)
                {
                    iter.Status.AddBurf(burfs.ToList()[1]);
                }
                else
                {
                    iter.Status.AddBurf(burfs.ToList()[0]);
                }
            }
        }
    }
}
