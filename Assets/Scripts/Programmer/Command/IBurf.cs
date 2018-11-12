using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IBurf
    {
        int RemainingTurn
        {
            get; set;
        }

        string Description
        {
            get;
        }

        string IconName
        {
            get;
        }

        bool IsPersistent
        {
            get;
        }

        bool IsPositiveBurf
        {
            get;
        }
    }
}
