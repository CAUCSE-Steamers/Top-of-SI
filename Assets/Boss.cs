using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Boss : MonoBehaviour {
    //private List<BossSkill> skills;
    BossSkill update, requirementchanged;
    private Animator anim;

    // Use this for initialization
    void Start () {
        //skills = new List<BossSkill>();
        anim = GetComponent<Animator>();
        update = gameObject.AddComponent<BossSkill_Update>();
        requirementchanged = gameObject.AddComponent<BossSkill_RequirementChanged>();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            update.Do(anim);
            Debug.Log("Space Pressed");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            requirementchanged.Do(anim);
            Debug.Log("Left Control Pressed");
        }
        */
        update.Do(anim);
    }
}
