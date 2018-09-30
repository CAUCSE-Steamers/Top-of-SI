using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class BossSkill_RequirementChanged : MonoBehaviour, Assets.IBossSkill
{
    void IBossSkill.Do(ref Animator anim)
    {
        anim.Play("shout");
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
