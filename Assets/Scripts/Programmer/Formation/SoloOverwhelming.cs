using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class SoloOverwhelming : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0)
        };

        public SoloOverwhelming() : base("장판파", 1)
        {
            burfs = new List<IBurf>
            {
                new SkillDamageBurf(0.2),
                new NormalAttackDamageBurf(0.5),
                new HealBurf(2, 3)
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
            var programmer = programmers.Single();
            foreach (var burf in burfs)
            {
                programmer.RegisterBurf(burf);
            }
        }

        public override void DetachBurfs()
        {
            var programmer = AffectedProgrammers.Single();
            foreach(var burf in burfs)
            {
                programmer.Status.RemoveBurf(burf);
            }
        }
    }
}
