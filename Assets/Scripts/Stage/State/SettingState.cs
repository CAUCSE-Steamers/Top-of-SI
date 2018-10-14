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

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateBool(StateParameter.Setting, false);
    }
}
