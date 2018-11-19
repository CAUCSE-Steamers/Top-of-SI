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

    public void Burf(List<BurfStructure> burf)
    {
        foreach (var iter in burf)
        {
            switch (iter.Type)
            {
                case BurfType.Cure:
                    int healedHP = (int)(Status.FullHealth * iter.Factor);
                    Status.Health = Mathf.Clamp(Status.Health + healedHP, 0, Status.FullHealth);
                    CommonLogger.Log("Boss Cured " + healedHP + "\n");
                    break;
                default:
                    Status.Burf.Add(iter);
                    break;
            }
        }
    }

    public ProjectSkill Invoke()
    {
        ProjectSkill ret = Ability.InvokedSkill;
        anim.Play(ret.Information.Animation);
        ret.ResetCoolDown();
        return ret;
    }

    public void InvokeFinished()
    {
        OnActionFinished();
    }
}