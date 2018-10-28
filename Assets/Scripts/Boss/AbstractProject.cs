using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public abstract class AbstractProject : MonoBehaviour, IHurtable, IInvokeSkills
{
    public event Action OnActionFinished = delegate { };

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

    public ProjectSkill Invoke()
    {
        ProjectSkill ret = Ability.InvokedSkill;
        anim.Play(ret.Information.Animation);

        OnActionFinished();
        return ret;
    }
}
