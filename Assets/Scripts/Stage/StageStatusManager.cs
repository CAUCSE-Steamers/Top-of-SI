﻿using UnityEngine;
using System;
using System.Collections;
using Model;
using System.Linq;

public class StageStatusManager : MonoBehaviour, IEventDisposable
{
    public event Action<StageStatus> OnStatusChanged = delegate { };
    public event Action<int> OnElapsedDayChanged = delegate { };
    public event Action<Direction> OnStageDirectionChanged = delegate { };

    private int elapsedDays;
    private StageStatus currentStatus;
    private Direction stageDirection;
    private UnitManager unitManager;

    public Direction StageDirection
    {
        get
        {
            return stageDirection;
        }
        set
        {
            stageDirection = value;
            OnStageDirectionChanged(stageDirection);
        }
    }

    public int MaximumDayLimit
    {
        get; set;
    }

    public void InitializeStageStatus(int maximumDayLimit, UnitManager unitManager)
    {
        CommonLogger.Log("StageStatusManager::InitializeStageStatus => 초기화 시작");

        OnElapsedDayChanged += SetToGameOverIfDayExceeded;

        unitManager.OnTurnChanged += SetToGameOverIfDayIsEqualToLimitAndTurnChangedToBoss;
        unitManager.OnTurnChanged += IncreaseDayIfTurnChangedToPlayer;
        OnStatusChanged += SetTurnStageToEnd;
        OnStatusChanged += ForceReturningFromVacationWhenStageFinished;
        OnStatusChanged += ForceUnapplyAllBurfsWhenStageFinished;

        this.MaximumDayLimit = maximumDayLimit;
        this.unitManager = unitManager;

        StageDirection = Direction.Right;
        CurrentStatus = StageStatus.InProgress;

        ElapsedDays = 0;

        CommonLogger.Log("StageStatusManager::InitializeStageStatus => 초기화 완료");
    }

    private void SetTurnStageToEnd(StageStatus turn)
    {
        if (turn != StageStatus.InProgress)
        {
            unitManager.Turn = TurnState.GameEnd;
        }
    }

    public void RegisterEventAfterInit(UnitManager unitManager)
    {
        unitManager.Boss.OnDeath += SetToStageClear;
        unitManager.OnTurnChanged += DecreaseAvailableVacationIfTurnChangedToPlayer;
    }

    private void SetToGameOverIfDayExceeded(int currentDays)
    {
        if (currentDays > MaximumDayLimit)
        {
            CommonLogger.LogFormat("StageStatusManager::SetToGameOverIfDayExceeded => 진행 일시가 {0}일을 초과함. 게임 오버!", MaximumDayLimit);
            CurrentStatus = StageStatus.Failure;
            StageManager.Instance.StageUi.TransitionToFailure(string.Format("프로젝트 진행 일시가 {0}일을 초과했습니다.", MaximumDayLimit));
        }
    }

    private void SetToGameOverIfDayIsEqualToLimitAndTurnChangedToBoss(TurnState turn)
    {
        if (turn == TurnState.Boss && ElapsedDays == MaximumDayLimit)
        {
            CommonLogger.LogFormat("StageStatusManager::SetToGameOverIfDayExceeded => 진행 일시가 최대 제한 일수인 {0}일과 같은 상태에서 플레이어 턴이 종료됨. 게임 오버!", ElapsedDays);
            CurrentStatus = StageStatus.Failure;
            StageManager.Instance.StageUi.TransitionToFailure(string.Format("프로젝트 진행 일시가 {0}일을 초과했습니다.", MaximumDayLimit));
        }
    }

    private void SetToStageClear()
    {
        CurrentStatus = StageStatus.Victory;

        int reward = StageManager.Instance.CurrentStage.Reward;

        StageManager.Instance.StageUi.TransitionToVictory("프로젝트 기한 내에 프로젝트를 완수했습니다!",
            string.Format("프로젝트 완수로 {0} 골드를 획득했습니다!", reward));

        LobbyManager.Instance.CurrentPlayer.Money += reward;
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

    public void setDayLimit(double ratio)
    {
        this.MaximumDayLimit = (int)(this.MaximumDayLimit * (1 - ratio) + 0.5);
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
        OnStageDirectionChanged = delegate { };

        CommonLogger.Log("StageStatusManager::DisposeRegisteredEvents => 이벤트 Disposing 완료.");
    }

    private void DecreaseAvailableVacationIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            int forcedReturnedCount = 0;

            foreach (var programmer in unitManager.Programmers.Where(programmer => programmer.Status.IsOnVacation).ToList())
            {
                --programmer.Status.RemainingVacationDay;

                if (programmer.Status.RemainingVacationDay <= 0)
                {
                    CommonLogger.LogFormat("StageStatusManager::DecreaseAvailableVacationIfTurnChangedToPlayer => 잔여 휴가 일수가 끝나서 {0}가 휴가에서 강제로 돌아옴.", programmer.Status.Name);

                    programmer.ReturnFromVacation(ElapsedDays, false);
                    StageManager.Instance.StageUi.ChangeProgrammerAlphaColor(programmer, 1f);

                    ++forcedReturnedCount;
                }

                if (forcedReturnedCount > 0)
                {
                    StageManager.Instance.StageUi.RenderPlayerText(string.Format("프로그래머 {0}명이 잔여 휴가 일수가 없어 근무로 돌아옵니다.", forcedReturnedCount));
                }
            }
        }
    }

    private void ForceReturningFromVacationWhenStageFinished(StageStatus stageStatus)
    {
        if (stageStatus != StageStatus.InProgress)
        {
            foreach (var programmer in unitManager.Programmers.Where(programmer => programmer.Status.IsOnVacation).ToList())
            {
                CommonLogger.LogFormat("StageStatusManager::ForceReturningFromVacationWhenStageFinished => 스테이지가 종료되어 {0}가 휴가에서 강제로 돌아옴.", programmer.Status.Name);
                programmer.ReturnFromVacation(ElapsedDays);

                StageManager.Instance.StageUi.ChangeProgrammerAlphaColor(programmer, 1f);
            }
        }
    }

    private void ForceUnapplyAllBurfsWhenStageFinished(StageStatus stageStatus)
    {
        if (stageStatus != StageStatus.InProgress)
        {
            CommonLogger.LogFormat("StageStatusManager::ForceUnapplyAllBurfsWhenStageFinished => 스테이지가 종료되어 버프가 강제로 해제됨.");

            foreach (var programmer in unitManager.Programmers)
            {
                foreach (var burf in programmer.Status.Burfs)
                {
                    programmer.UnregisterBurf(burf);
                }
            }
        }
    }
}