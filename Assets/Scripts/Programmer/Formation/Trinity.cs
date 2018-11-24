using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class Trinity : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -2)
        };

        public Trinity() : base("트리니티", 3)
        {
            burfs = new List<IBurf>
            {
                new DamageIncreaseDeburf(0.2) { RemainingTurn = int.MaxValue },
                new HealBurf(3, 5) { RemainingTurn = int.MaxValue },
                new SkillCooldownBurf(-0.2) { RemainingTurn = int.MaxValue }
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
