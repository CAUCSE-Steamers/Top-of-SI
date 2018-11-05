using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IStatusModificationCommand
    {
        void Modify(ProgrammerStatus status);
        void Unmodify(ProgrammerStatus status);
    }
}
