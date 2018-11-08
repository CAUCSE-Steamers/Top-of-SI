using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Money
    {
        public Money(int hire, int pay, int fire)
        {
            Hire = hire;
            Pay = pay;
            Fire = fire;
        }
        public int Hire
        {
            get; private set;
        }
        public int Pay
        {
            get; private set;
        }
        public int Fire
        {
            get; private set;
        }
    }
}
