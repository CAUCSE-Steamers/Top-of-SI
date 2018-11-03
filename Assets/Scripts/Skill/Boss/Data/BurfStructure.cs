using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BurfStructure
    {
        public BurfStructure(BurfType type, int turn, double factor)
        {
            Type = type;
            Turn = turn;
            Factor = factor;
        }
        public BurfType Type
        {
            get; private set;
        }

        public int Turn
        {
            get; private set;
        }

        public double Factor
        {
            get; private set;
        }

        public void DecreaseTurn()
        {
            Turn--;
        }
    }
}
