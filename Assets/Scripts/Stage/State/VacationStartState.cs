using UnityEngine;
using System.Collections;

public class VacationStartState : DispatchableState
{
    public Programmer SelectedProgrammer
    {
        get; private set;
    }

    public void SetSelectedProgrammer(Programmer programmer)
    {
        if (programmer == null)
        {
            DebugLogger.LogError("SelectMovingCellState::SetSelectedProgrammer => 주어진 프로개르머가 Null입니다.");
        }

        SelectedProgrammer = programmer;
    }

    public void ConfirmVacation()
    {
        if (SelectedProgrammer != null)
        {
            SelectedProgrammer.GoVacation(Manager.Status.ElapsedDays);
        }
    }

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.Idle);
    }
}
