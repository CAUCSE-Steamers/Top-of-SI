using UnityEngine;
using System.Collections;

public static class StageStateUtility
{
    public static void SetStateTrigger(this Animator animator, StateParameter stateParameter)
    {
        animator.SetTrigger(stateParameter.ParameterToName());
    }

    public static void SetStateBool(this Animator animator, StateParameter stateParameter, bool value)
    {
        animator.SetBool(stateParameter.ParameterToName(), value);
    }

    public static bool GetStateBool(this Animator animator, StateParameter stateParameter)
    {
        return animator.GetBool(stateParameter.ParameterToName());
    }
}
