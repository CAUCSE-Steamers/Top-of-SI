using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SkillBasicInformation
    {
        public SkillType Type
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int AcquisitionLevel
        {
            get; set;
        }

        public int MaximumLevel
        {
            get; set;
        }
    }
}
