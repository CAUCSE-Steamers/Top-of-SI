using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CooldownComparer : IComparer<ICooldownRequired>
    {
        public int Compare(ICooldownRequired x, ICooldownRequired y)
        {
            return x.DefaultCooldown.CompareTo(y);
        }
    }
}
