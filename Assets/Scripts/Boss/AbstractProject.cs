using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public abstract class AbstractProject : MonoBehaviour, IHurtable, IInvokeSkills
{
    private Boolean isOverwhelming = false, isDecreaseDamage = false;
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
                    Status.Health += healedHP;
                    CommonLogger.Log("Boss Cured " + healedHP + "\n");
                    break;
                case BurfType.DecreaseDamage:
                    // Pleace Check this before apply damage
                    isDecreaseDamage = true;
                    CommonLogger.Log("Damage is decreased " + iter.Factor * 100 + "%\n");
                    break;
                case BurfType.Overwhelming:
                    // Please Check this before apply damage
                    isOverwhelming = true;
                    CommonLogger.Log("Boss is Overwhelming during " + iter.Turn + "turns\n");
                    break;
            }
        }
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