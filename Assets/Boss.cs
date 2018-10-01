using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Boss : AbstractBoss {
    //private List<BossSkill> skills;
    //pririty queue 구현 필요

    private Animator anim;
    readonly int skill_hash = Animator.StringToHash("Skill_Called");
    
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
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
            anim.Play("Attack");
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.Play("Shout");
        }
    }
}
