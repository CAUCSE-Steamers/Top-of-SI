using UnityEngine;
using System;
using System.Collections;

public class IdleState : DispatchableState
{
    public event Action<GameObject> OnSelected = delegate { };
    private GameObject reservedObject = null;

    public void ResetSelectedObject()
    {
        SelectedObject = null;
    }

    public void ReserveSetSelectedObject(GameObject obj)
    {
        reservedObject = obj;
    }

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
        ResetSelectedObject();

        if (reservedObject != null)
        {
            SelectedObject = reservedObject;
            reservedObject = null;
        }

        foreach (var programmer in Manager.Unit.Programmers)
        {
            programmer.OnMouseClicked += ChangeSelectedProgrammer;
        }
    }

    public void AddProgrammerEvent(Programmer programmer)
    {
        programmer.OnMouseClicked += ChangeSelectedProgrammer;
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

    public void TransitionToVacationState()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.StartVacation);
    }

    public void TransitionToPauseState()
    {
        var state = PlayingAnimator.GetBehaviour<PauseState>();

        if (SelectedObject != null)
        {
            state.SelectedProgrammer = SelectedObject.GetComponent<Programmer>();
        }

        PlayingAnimator.SetStateBool(StateParameter.Pause, true);
    }

    public void TransitionToSettingState()
    {
        var state = PlayingAnimator.GetBehaviour<SettingState>();

        if (SelectedObject != null)
        {
            state.SelectedProgrammer = SelectedObject.GetComponent<Programmer>();
        }

        PlayingAnimator.SetStateBool(StateParameter.Setting, true);
    }

    public void TransitionToFailureState()
    {
        PlayingAnimator.SetStateBool(StateParameter.Failure, true);
    }

    public void TransitionToVictoryState()
    {
        PlayingAnimator.SetStateBool(StateParameter.Victory, true);
    }
}
