using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public abstract class AbstractProject : MonoBehaviour, IHurtable, IInvokeSkills
{
    public event Action OnActionFinished = delegate { };
    public event Action OnDeath = delegate { };

    protected Animator anim;
    public ProjectStatus Status
    {
        get; protected set;
    }

    public ProjectAbility Ability
    {
        get; protected set;
    }

    public abstract void Hurt(int damage);

    protected void InvokeDeathEvent()
    {
        OnDeath();
    }

    public ProjectSkill Invoke()
    {
        ProjectSkill ret = Ability.InvokedSkill;
        anim.Play(ret.Information.Animation);
        ret.ResetCoolDown();
        OnActionFinished();
        return ret;
    }
}
