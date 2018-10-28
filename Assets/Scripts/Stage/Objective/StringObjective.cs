using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class StringObjective : IStageObjective
    {
        public StringObjective(string description)
        {
            Description = description;
        }

        public string Description
        {
            get; private set;
        }
    }
}
