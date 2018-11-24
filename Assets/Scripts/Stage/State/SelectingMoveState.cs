using UnityEngine;
using System.Collections.Generic;
using System;

public class SelectingMoveState : DispatchableState
{
    public event Action OnMovingStarted = delegate { };

    private bool isBlockMoving;

    public Programmer SelectedProgrammer
    {
        get; private set;
    }


    protected override void ProcessEnterState()
    {
        isBlockMoving = false;
    }

    protected override void ProcessExitState()
    {
        foreach (var cell in Manager.StageField.Cells)
        {
            cell.OnMouseClicked -= DisableCellEffect;
            cell.OnMouseClicked -= MoveProgrammerToCell;
        }

        SelectedProgrammer.OnActionFinished -= TransitionToIdle;
    }

    public void SetSelectedProgrammer(Programmer programmer)
    {
        if (programmer == null)
        {
            DebugLogger.LogError("SelectMovingCellState::SetSelectedProgrammer => 주어진 프로개르머가 Null입니다.");
        }

        SelectedProgrammer = programmer;
        programmer.OnActionFinished += TransitionToIdle;

        foreach (var movableCell in Manager.Unit.CurrentMovableCellFor(SelectedProgrammer))
        {
            movableCell.OnMouseClicked += DisableCellEffect;
            movableCell.OnMouseClicked += MoveProgrammerToCell;

            movableCell.SetEffectActiveState(true);
        }
    }

    public void DisableCellEffect(GameObject selectedObject)
    {
        Manager.StageField.SetAllCellEffectOff();
    }

    private void MoveProgrammerToCell(GameObject selectedObject)
    {
        if (isBlockMoving == false)
        {
            OnMovingStarted();

            var cellComponent = selectedObject.GetComponent<Cell>();

            if (cellComponent != null)
            {
                var programmerFieldPosition = Manager.StageField.VectorToIndices(SelectedProgrammer.transform.position);
                var positionDifference = cellComponent.PositionInField - programmerFieldPosition;

                var vectorDifference = Manager.StageField.IndicesTransformToVector(positionDifference);

                SelectedProgrammer.Move(vectorDifference);
            }
        }
    }

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.Idle);
        isBlockMoving = true;
    }
}
