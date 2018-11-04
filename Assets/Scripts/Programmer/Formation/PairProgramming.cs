using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model.Formation
{
    public class PairProgramming : Formation
    {
        private static readonly IEnumerable<Vector2Int> relativeFormationLocations = new List<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(-1, 0)
        };

        public PairProgramming() 
            : base("페어 프로그래밍", 2)
        {
        }

        protected override IEnumerable<Vector2Int> RelativeFormation
        {
            get
            {
                return relativeFormationLocations;
            }
        }

        protected override IEnumerable<BurfStructure> FormationsBurfs
        {
            get
            {
                return null;
            }
        }
    }
}
