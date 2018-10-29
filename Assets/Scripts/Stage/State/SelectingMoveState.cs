using UnityEngine;
using System.Collections.Generic;
using System;

public class SelectingMoveState : DispatchableState
{
    public event Action OnMovingStarted = delegate { };

    private Programmer selectedProgrammer;

    protected override void ProcessExitState()
    {
        foreach (var cell in Manager.StageField.Cells)
        {
            cell.OnMouseClicked -= DisableCellEffect;
            cell.OnMouseClicked -= MoveProgrammerToCell;
        }

        selectedProgrammer.OnActionFinished -= TransitionToIdle;
    }

    public void SetSelectedProgrammer(Programmer programmer)
    {
        if (programmer == null)
        {
            DebugLogger.LogError("SelectMovingCellState::SetSelectedProgrammer => 주어진 프로개르머가 Null입니다.");
        }

        selectedProgrammer = programmer;
        programmer.OnActionFinished += TransitionToIdle;

        foreach (var movableCell in Manager.Unit.CurrentMovableCellFor(selectedProgrammer))
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
        OnMovingStarted();

        var cellComponent = selectedObject.GetComponent<Cell>();

        if (cellComponent != null)
        {
            var programmerFieldPosition = Manager.StageField.VectorToIndices(selectedProgrammer.transform.position);
            var positionDifference = cellComponent.PositionInField - programmerFieldPosition;

            var vectorDifference = Manager.StageField.IndicesTransformToVector(positionDifference);

            selectedProgrammer.Move(vectorDifference);
        }
    }

    public void TransitionToIdle()
    {
        PlayingAnimator.SetStateTrigger(StateParameter.Idle);
    }
}
