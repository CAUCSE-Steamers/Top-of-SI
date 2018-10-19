﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public abstract class ProjectSkill : ICooldownRequired, IComparable<ProjectSkill>
    {
        private double cooldown;

        public ProjectSkill(ProjectSkillInformation information, double baseDamage, double defaultCooldown)
        {
            Information = information;
            BaseDamage = baseDamage;
            DefaultCooldown = defaultCooldown;

            cooldown = 0;
        }

        public bool IsTriggerable
        {
            get
            {
                return cooldown < double.Epsilon;
            }
        }

        public void DecreaseCooldown()
        {
            if(cooldown >= 1.0)
            {
                cooldown -= 1.0;
            }
        }

        public int CompareTo(ProjectSkill other)
        {
            throw new NotImplementedException();
        }

        public ProjectSkillInformation Information
        {
            get; private set;
        }

        public double BaseDamage
        {
            get; private set;
        }

        public double DefaultCooldown
        {
            get; private set;
        }

        
    }
}
