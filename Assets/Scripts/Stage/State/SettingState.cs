using UnityEngine;
using System.Collections;

public class SettingState : DispatchableState
{
    protected override void ProcessEnterState()
    {
        Time.timeScale = 0.0f;
    }

    protected override void ProcessExitState()
    {
        Time.timeScale = 1.0f;
    }

    public Programmer SelectedProgrammer
    {
        get; set;
    }

    public void TransitionToIdle()
    {
        if (SelectedProgrammer != null)
        {
            var state = PlayingAnimator.GetBehaviour<IdleState>();
            state.ReserveSetSelectedObject(SelectedProgrammer.gameObject);
        }

        PlayingAnimator.SetStateBool(StateParameter.Setting, false);
    }
}
