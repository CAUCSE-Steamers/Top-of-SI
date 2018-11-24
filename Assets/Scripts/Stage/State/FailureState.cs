using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Model;

public class FailureState : DispatchableState
{
    protected override void ProcessEnterState()
    {
        StageManager.Instance.Status.CurrentStatus = StageStatus.Failure;
    }
}
