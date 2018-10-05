using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Boss : AbstractBoss {

    private Animator anim;
    readonly int skill_hash = Animator.StringToHash("Skill_Called");
    
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        this.skill_list = new List<AbstractSkill>();
        //nullpoint exception
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
            Debug.Log("LeftControl Pressed");
            skill_list[0].Do(ref anim);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("LeftShift Pressed");
            skill_list[1].Do(ref anim);
        }
    }

    public override void Do()
    {
        //Boss AI function
        throw new System.NotImplementedException();
    }
}
