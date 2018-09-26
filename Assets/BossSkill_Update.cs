using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class BossSkill_Update : MonoBehaviour, Assets.BossSkill {
    private Animator anim;

    void BossSkill.Do() {
        anim.Play("Attack");
    }

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.Play("Attack");
        }
        // Temporary If for testing Animation.
        // 1. Add Damage Affect to Character in BossSkill.Do
    }
}
