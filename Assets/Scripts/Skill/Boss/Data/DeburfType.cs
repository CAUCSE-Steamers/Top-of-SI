using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum DeburfType
    {
        None = 0, 
        ShortenDeadLine = 1, 
        IncreaseMentalUsage = 2, 
        DisableMovement = 4, 
        DecreaseAttack = 8, 
        IncreaseDamage = 16, 
        Splash = 32
    }
}
