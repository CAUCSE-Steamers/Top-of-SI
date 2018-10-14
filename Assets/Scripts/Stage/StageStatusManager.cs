using UnityEngine;
using System;
using System.Collections;
using Model;

public class StageStatusManager : MonoBehaviour, IEventDisposable
{
    public event Action<StageStatus> OnStatusChanged = delegate { };
    public event Action<int> OnElapsedDayChanged = delegate { };

    private int maximumDayLimit;
    private int elapsedDays;
    private StageStatus currentStatus;
    private UnitManager unitManager;
    
    public void InitializeStageStatus(int maximumDayLimit, UnitManager unitManager)
    {
        CommonLogger.Log("StageStatusManager::InitializeStageStatus => 초기화 시작");

        OnElapsedDayChanged += SetToGameOverIfDayExceeded;

        unitManager.OnTurnChanged += SetToGameOverIfDayIsEqualToLimitAndTurnChangedToBoss;
        unitManager.OnTurnChanged += IncreaseDayIfTurnChangedToPlayer;
        this.maximumDayLimit = maximumDayLimit;
        this.unitManager = unitManager;

        CurrentStatus = StageStatus.InProgress;
        ElapsedDays = 0;
        
        CommonLogger.Log("StageStatusManager::InitializeStageStatus => 초기화 완료");
    }
    
    private void SetToGameOverIfDayExceeded(int currentDays)
    {
        if (currentDays > maximumDayLimit)
        {
            CommonLogger.LogFormat("StageStatusManager::SetToGameOverIfDayExceeded => 진행 일시가 {0}일을 초과함. 게임 오버!", maximumDayLimit);
            CurrentStatus = StageStatus.Failure;
        }
    }

    private void SetToGameOverIfDayIsEqualToLimitAndTurnChangedToBoss(TurnState turn)
    {
        if (turn == TurnState.Boss && ElapsedDays == maximumDayLimit)
        {
            CommonLogger.LogFormat("StageStatusManager::SetToGameOverIfDayExceeded => 진행 일시가 최대 제한 일수인 {0}일과 같은 상태에서 플레이어 턴이 종료됨. 게임 오버!", ElapsedDays);
        }
    }

    public StageStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
        set
        {
            CommonLogger.LogFormat("StageStatusManager::CurrentStatus => 스테이지 상태가 '{0}'으로 바뀌려 함.", value);

            currentStatus = value;
            OnStatusChanged(currentStatus);
        }
    }

    private void IncreaseDayIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            ElapsedDays += 1;
        }
    }
    
    public int ElapsedDays
    {
        get
        {
            return elapsedDays;
        }
        set
        {
            CommonLogger.LogFormat("StageStatusManager::ElapsedDays => 진행 일시가 {0}으로 바뀌려 함.", value);

            elapsedDays = value;
            OnElapsedDayChanged(elapsedDays);
        }
    }

    public void DisposeRegisteredEvents()
    {
        OnStatusChanged = delegate { };
        OnElapsedDayChanged = delegate { };

        CommonLogger.Log("StageStatusManager::DisposeRegisteredEvents => 이벤트 Disposing 완료.");
    }

    public void GoVacation()
    {
        var selectedProgrammerStatus = unitManager.CurrentSelectedProgrammer.Status;

        if (selectedProgrammerStatus.IsOnVacation)
        {
            DebugLogger.LogWarningFormat("프로그래머 '{0}'는 이미 휴가를 떠난 상태지만, 또 휴가를 떠나려고 합니다.", unitManager.CurrentSelectedProgrammer.name);
        }

        selectedProgrammerStatus.StartVacationDay = ElapsedDays;
    }

    public void ReturnFromVacation()
    {
        var selectedProgrammerStatus = unitManager.CurrentSelectedProgrammer.Status;

        if (selectedProgrammerStatus.IsOnVacation == false)
        {
            DebugLogger.LogWarningFormat("프로그래머 '{0}'는 휴가를 떠나지 않은 상태에서 복귀하려고 합니다.", unitManager.CurrentSelectedProgrammer.name);
        }

        int deltaDays = (ElapsedDays - selectedProgrammerStatus.StartVacationDay).Value;
        int healQuantity = (int) ((selectedProgrammerStatus.FullHealth * (0.05 * deltaDays * deltaDays)));
        unitManager.CurrentSelectedProgrammer.Heal(healQuantity);
        selectedProgrammerStatus.StartVacationDay = null;
    }
}