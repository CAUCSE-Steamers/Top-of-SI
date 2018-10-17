using UnityEngine;
using System.Collections;

public class VacationStartState : DispatchableState
{
    private Programmer selectedProgrammer;

    public void SetSelectedProgrammer(Programmer programmer)
    {
        if (programmer == null)
        {
            DebugLogger.LogError("SelectMovingCellState::SetSelectedProgrammer => 주어진 프로개르머가 Null입니다.");
        }

        selectedProgrammer = programmer;
    }

    public void ConfirmVacation()
    {
        if (selectedProgrammer != null)
        {
            selectedProgrammer.GoVacation(Manager.Status.ElapsedDays);
        }
    }

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.Idle);
    }
}
