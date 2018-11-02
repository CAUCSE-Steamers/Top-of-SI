using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DeBurfStructure
    {
        public DeBurfStructure(DeburfType type, int turn, double factor)
        {
            Type = type;
            Turn = turn;
            Factor = factor;
        }
        public DeburfType Type
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

    }
}
