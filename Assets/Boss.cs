using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Boss : AbstractBoss {

    private Animator anim;
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        //nullpoint exception
        this.skill_list = new List<AbstractSkill>();
        if(GetComponent<BossSkill_RequirementChanged>() == null)
        {
            gameObject.AddComponent<BossSkill_RequirementChanged>();
        }
        if(GetComponent<BossSkill_Update>() == null)
        {
            gameObject.AddComponent<BossSkill_Update>();
        }
        skill_list.Add(GetComponent<BossSkill_RequirementChanged>());
        skill_list.Add(GetComponent<BossSkill_Update>());
        Debug.Log("HP : " + GetCurrentHP());
    }
	
	// Update is called once per frame
	void Update () {
        // This codes are added for test of Get Damage.
        // After merge, please Delete it.

        if (Input.GetKey(KeyCode.Space))
        {
            bool die = GetDamage(10);
            if (die)
            {
                Debug.Log("Boss Dead");
            }
            else
            {
                Debug.Log("boss get Damaged. now HP is " + GetCurrentHP());
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            this.Do();
        }
    }

    public override void Do()
    {
        //Add Cool and Sort
        foreach (AbstractSkill skill in skill_list)
        {
            skill.OnTurn();
        }
        skill_list.Sort((x, y) => x.CompareTo(y));
        //Do Skill
        foreach(AbstractSkill skill in skill_list)
        {
            if (skill.SkillOnActive())
            {
                skill.Do(ref anim);
            }
        }
    }
}
