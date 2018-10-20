using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum RequiredTechType
    {
        Web = 0,
        Server = 1,
        Android = 2,
        Network = 4,
        Desktop = 8,
        Embedded = 16,
        Database = 32,
        DataAnalysis = 64,
        Ai = 128,
        Ui = 256,
        Compiler = 512,
        Multiprogramming = 1024,
        Graphic = 2048, 
        Common = 4096
    }
}
