using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System;

public abstract class AbstractSkill : MonoBehaviour, IComparable<AbstractSkill>{
    protected int coolTime = 0, cool = 0;
    private List<IPassive> passiveSkills;


	// Use this for initialization
	void Start () {
        passiveSkills = new List<IPassive>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTurn()
    {
        cool++;
    }

    public bool SkillOnActive()
    {
        return cool >= this.GetCoolTime();
    }

    public int GetCoolTime()
    {
        float cooltime = this.coolTime;
        if(passiveSkills == null)
        {
            return (int)(cooltime);
        }
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

    public int CompareTo(AbstractSkill other)
    {
        if(this.coolTime == other.coolTime)
        {
            return (this.cool > other.cool) ? 1 : -1;
        }
        else
        {
            return (this.GetCoolTime() > other.GetCoolTime()) ? 1 : -1;
        }
    }
}
