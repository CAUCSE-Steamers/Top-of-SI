using UnityEngine;
using System.Collections;

public class ProgrammerSelectedState : DispatchableState
{
    public void TransitionToMove()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.SelectMove);
    }
}
