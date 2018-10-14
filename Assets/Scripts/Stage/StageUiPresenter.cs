using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class StageUiPresenter : MonoBehaviour
{
    [SerializeField]
    private Animator stateAnimator;
    [SerializeField]
    private ObjectInformationPresenter objectInformationPresenter;

    public void GoToSelectingMoveCell()
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();

        if (idleState.IsProgrammerSelected)
        {
            var programmer = idleState.SelectedObject.GetComponent<Programmer>();
            var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
            moveState.SetSelectedProgrammer(programmer);

            idleState.TransitionToMoveState();
        }
    }

    public void CancelMove()
    {
        var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
        moveState.DisableCellEffect(gameObject);
        moveState.TransitionToIdle();
    }

    public void AddEnterEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnEntered += action;
        }
    }

    public void AddUpdateEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnUpdated += action;
        }
    }

    public void AddExitEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnExit += action;
        }
    }
}
