using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum DeburfType
    {
        None = 1, 
        ShortenDeadLine = 2, 
        IncreaseMentalUsage = 4, 
        DisableMovement = 8, 
        DecreaseDamage = 16
    }
}
