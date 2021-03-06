﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class Boss : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(2, -1),
            new Vector2Int(2, 0),
            new Vector2Int(2, 1)
        };

        public Boss() : base("보스", 4)
        {
            burfs = new List<IBurf>
            {
                new DamageSplashBurf(0.0) { RemainingTurn = int.MaxValue },
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
                    programmer.RegisterBurf(burfs.ElementAt(0));
                }
                else
                {
                    programmer.RegisterBurf(burfs.ElementAt(1));
                }
            }
        }
    }
}
