using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public abstract class AbstractSkill : MonoBehaviour{
    protected int coolTime = 0, cool = 0;
    private List<IPassive> passiveSkills;


	// Use this for initialization
	void Start () {
        passiveSkills = new List<IPassive>();
	}
	
	// Update is called once per frame
	void Update () {
        cool++;
	}

    public bool SkillOnActive()
    {
        return cool >= this.GetCoolTime();
    }

    public int GetCoolTime()
    {
        float cooltime = this.coolTime;
        foreach (IPassive iter in passiveSkills)
        {
            cooltime = iter.SkilledCoolTime(cooltime);
        }
        return (int)(cooltime + 0.5);
    }

    public void AddPassiveSkills(IPassive passive)
    {
        passiveSkills.Add(passive);
    }

    public abstract void Do(ref Animator anim);
}
