using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class TestProject : AbstractProject
    {
        public TestProject()
        {
            Status = new ProjectStatus
            {
                Name = "TestBoss",
                FullHealth = 1000,
                Health = 1000
            };
            List<ProjectSkill> skill_list = new List<ProjectSkill>
            {
                new VersionUpdate(),
                new RequirementChanged(),
                new DeadLineChanged()
            };
            Ability = new ProjectAbility(skill_list, ProjectType.None);
        }

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void Hurt(int damage)
        {
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
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