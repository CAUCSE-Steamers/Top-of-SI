using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

namespace Model
{
    public class TestProject : AbstractProject
    {
        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            Status = new ProjectStatus
            {
                Name = "TestBoss",
                FullHealth = 100,
                Health = 100
            };
            List<ProjectSkill> skill_list = new List<ProjectSkill>();
            skill_list.Add(new VersionUpdate());
            skill_list.Add(new RequirementChanged());
            Ability = new ProjectAbility(skill_list, ProjectType.None);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void Hurt(int damage)
        {
            anim.Play("Get_Hit");
            Status.Health -= damage;
        }
    }
}