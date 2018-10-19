using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum ProjectType
    {
        None = 0,
        Client = 1,
        Application = 2,
        Server = 4,
        IoT = 8
    }

    public enum TechniqueType
    {
        Common = 0,
        Web = 1,
        Server = 2,
        Android = 4,
        Network = 8,
        DesktopApp = 16,
        Embedded = 32,
        DB = 64,
        DataAnalysis = 128,
        UI = 256,
        AI = 512,
        Compiler = 1024,
        MultiProgramming = 2048,
        Graphics = 4096
    }
}
