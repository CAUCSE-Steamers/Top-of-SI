using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IConstantAcceptableCommand<in T>
    {
        void Accept(T value);
        void Leave(T value);
    }
}
