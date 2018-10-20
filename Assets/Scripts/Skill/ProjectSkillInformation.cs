using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ProjectSkillInformation
    {
        public ProjectSkillType Type
        {
            get; set;
        }

        public RequiredTechType Technique
        {
            get; set;
        }

        public String Name
        {
            get; set;
        }

        public int MaximumLevel
        {
            get; set;
        }

        public String Animation
        {
            get; set;
        }
    }
}
