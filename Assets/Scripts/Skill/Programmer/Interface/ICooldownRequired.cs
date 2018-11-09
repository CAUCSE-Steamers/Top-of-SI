using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface ICooldownRequired
    {
        double DefaultCooldown
        {
            get;
        }

        bool IsTriggerable
        {
            get;
        }

        void DecreaseCooldown();
    }
}
