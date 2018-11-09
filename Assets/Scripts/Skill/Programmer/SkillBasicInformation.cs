using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SkillBasicInformation
    {
        public Sprite Image
        {
            get
            {
                return ResourceLoadUtility.LoadIcon(IconName);
            }
        }

        public string IconName
        {
            get; set;
        }

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

        public int RequiredUpgradeCost
        {
            get; set;
        }
    }
}
