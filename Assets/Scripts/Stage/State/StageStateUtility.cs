using UnityEngine;
using System.Collections;

public static class StageStateUtility
{
    public static void SetStateTrigger(this Animator animator, StateParameter stateParameter)
    {
        animator.SetTrigger(stateParameter.ParameterToName());
    }
}
