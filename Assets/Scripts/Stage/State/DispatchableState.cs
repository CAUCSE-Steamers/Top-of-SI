using UnityEngine;
using UnityEngine.Animations;
using System;

public abstract class DispatchableState : StateMachineBehaviour
{
    public event Action OnEntered;
    public event Action OnUpdated;
    public event Action OnExit;

    protected Animator PlayingAnimator
    {
        get; private set;
    }

    protected StageManager Manager
    {
        get; private set;
    }

    private void OnEnable()
    {
        Manager = GameObject.Find("StageManager").GetComponent<StageManager>();
        PlayingAnimator = GameObject.Find("GameUi").GetComponent<Animator>();

        ClearEvent();
    }
    
    protected void ClearEvent()
    {
        OnEntered = null;
        OnUpdated = null;
        OnExit = null;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (OnEntered != null)
        {
            OnEntered();
        }

        PlayingAnimator = animator;
        Manager = StageManager.Instance;

        ProcessEnterState();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (OnUpdated != null)
        {
            OnUpdated();
        }

        ProcessUpdateState();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit != null)
        {
            OnExit();
        }

        ProcessExitState();
    }

    protected virtual void ProcessEnterState()
    {

    }

    protected virtual void ProcessUpdateState()
    {

    }

    protected virtual void ProcessExitState()
    {

    }
}