using UnityEngine;
using System;
using System.Collections;

public class IdleState : DispatchableState
{
    public event Action<GameObject> OnSelected = delegate { };

    public GameObject SelectedObject
    {
        get; private set;
    }

    public bool HasSelectedObject
    {
        get
        {
            return SelectedObject != null;
        }
    }

    public bool IsProgrammerSelected
    {
        get
        {
            return HasSelectedObject && SelectedObject.GetComponent<Programmer>() != null;
        }
    }

    protected override void ProcessEnterState()
    {
        SelectedObject = null;

        foreach (var programmer in Manager.Unit.Programmers)
        {
            programmer.OnMouseClicked += ChangeSelectedProgrammer;
        }
    }

    protected override void ProcessExitState()
    {
        foreach (var programmer in Manager.Unit.Programmers)
        {
            programmer.OnMouseClicked -= ChangeSelectedProgrammer;
        }

        SelectedObject = null;
        OnSelected = delegate { };
    }

    private void ChangeSelectedProgrammer(GameObject selectedObject)
    {
        var programmerComponent = selectedObject.GetComponent<Programmer>();
        if (programmerComponent != null)
        {
            SelectedObject = selectedObject;
        }

        OnSelected(SelectedObject);
    }

    public void TransitionToMoveState()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.SelectMove);
    }

    public void TransitionToPauseState()
    {
        PlayingAnimator.SetStateBool(StateParameter.Pause, true);
    }

    public void TransitionToSettingState()
    {
        PlayingAnimator.SetStateBool(StateParameter.Setting, true);
    }
}
