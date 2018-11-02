using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class SimpleSingingApplicationDataAnalysisTool : AbstractProject
    {
        void Start()
        {
            anim = GetComponent<Animator>();
            Status = new ProjectStatus
            {
                Name = "SimpleSingingApplicationDataAnalysisTool",
                //TODO : Check Programmer's Damage than resize FullHealth and Health
                FullHealth = 1000,
                Health = 1000
            };
            List<ProjectSkill> skill_list = new List<ProjectSkill>();
            //TODO : Add Skills
            Ability = new ProjectAbility(skill_list, ProjectType.Application);
        }
        public override void Hurt(int damage)
        {
            Status.Health = Mathf.Clamp(Status.Health - damage, 0, int.MaxValue);

            if (Status.Health <= 0)
            {
                anim.Play("Dead");
                InvokeDeathEvent();
            }
            else
            {
                anim.Play("Get_Hit");
            }
        }
    }
}
