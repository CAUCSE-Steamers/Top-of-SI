using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum RequiredTechType
    {
        None = 0,
        Web = 1,
        Server = 2,
        Android = 4,
        Network = 8,
        Desktop = 16,
        Embedded = 32,
        Database = 64,
        DataAnalysis = 128,
        Ai = 256,
        Ui = 512,
        Compiler = 1024,
        Multiprogramming = 2048,
        Graphic = 4096, 
        Common = 8192
    }
}
