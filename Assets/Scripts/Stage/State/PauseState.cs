using UnityEngine;
using System.Collections;
using System;

public class PauseState : DispatchableState
{
    protected override void ProcessEnterState()
    {
        Time.timeScale = 0.0f;
    }

    protected override void ProcessExitState()
    {
        Time.timeScale = 1.0f;
    }

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateBool(StateParameter.Pause, false);
    }
}
