using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class VictoryState : DispatchableState
{
    protected override void ProcessEnterState()
    {
        StageManager.Instance.Status.CurrentStatus = StageStatus.Victory;

        var currentStage = StageManager.Instance.CurrentStage;
        var currentPlayer = LobbyManager.Instance.CurrentPlayer;

        if (currentStage.MainStage)
        {
            ++currentPlayer.MainStageLevel;
        }

        currentPlayer.ClearedStageNames.Add(currentStage.Title);
    }
}
