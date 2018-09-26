using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class BossSkill_Update : MonoBehaviour, Assets.BossSkill {

    void BossSkill.Do(Animator anim) {
        anim.Play("Attack");
    }

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
    }
}
