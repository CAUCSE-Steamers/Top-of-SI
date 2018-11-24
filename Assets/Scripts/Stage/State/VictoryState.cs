using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class VictoryState : DispatchableState
{
    protected override void ProcessEnterState()
    {
        StageManager.Instance.Status.CurrentStatus = StageStatus.Failure;
    }
}
