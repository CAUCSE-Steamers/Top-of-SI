using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface ITupleAcceptableCommand<in T, in U>
    {
        void Accept(T first, U second);
        void Leave(T first, U second);
    }
}
