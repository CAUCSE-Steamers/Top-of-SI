using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Model;

public class UnitManager : MonoBehaviour, IDisposable, IEventDisposable
{
    public event Action<TurnState> OnTurnChanged = delegate { };

    // Programmer (Key) => Programmer has performed any action? (Value)
    private IDictionary<Programmer, bool> programmerActingDictionary;
    private Programmer boss; // TODO : Convert to boss
    private TurnState currentTurn;

    public void SetUnits(IEnumerable<Programmer> programmers, Programmer boss)
    {
        CommonLogger.Log("UnitManager::SetUnits => 초기화 시작.");

        if (programmers == null || boss == null)
        {
            DebugLogger.LogError("UnitManager::SetUnits => 주어진 프로그래머 또는 보스 중 Null이 존재함.");
            throw new ArgumentNullException();
        }

        OnTurnChanged += RequestBossActionIfTurnChangedToBoss;
        OnTurnChanged += PermitProgrammersActionIfTurnChangedToPlayer;
        
        programmerActingDictionary = 
            programmers.ToDictionary(keySelector: programmer => programmer,
                                     elementSelector: programmer => false);

        this.boss = boss;

        SubscribeToBoss();
        SubscribeToProgrammers();

        Turn = TurnState.Player;

        CommonLogger.Log("UnitManager::SetUnits => 초기화 완료.");
    }

    private void SubscribeToProgrammers()
    {
        foreach (var programmer in programmerActingDictionary.Keys)
        {
            programmer.OnActionFinished += () =>
            {
                programmerActingDictionary[programmer] = true;
                ChangeTurnToBossIfAllProgrammersPerformAction();
            };
        }
    }

    private void ChangeTurnToBossIfAllProgrammersPerformAction()
    {
        if (programmerActingDictionary.Values.All(actingState => actingState))
        {
            CommonLogger.Log("UnitManager::ChangeTurnToBossIfAllProgrammersPerformAction => 모든 프로그래머가 행동을 수행해 턴이 보스로 넘어감.");

            Turn = TurnState.Boss;
        }
    }

    private void SubscribeToBoss()
    {
        boss.OnActionFinished += () => Turn = TurnState.Player;
    }

    private void RequestBossActionIfTurnChangedToBoss(TurnState turn)
    {
        if (turn == TurnState.Boss)
        {
            CommonLogger.Log("UnitManager::RequestBossActionIfTurnChangedToBoss => 보스에게 행동을 요청함.");

            boss.UseSkill();
        }
    }

    private void PermitProgrammersActionIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            foreach (var programmer in programmerActingDictionary.Keys.ToArray())
            {
                programmerActingDictionary[programmer] = false;
            }

            CommonLogger.Log("UnitManager::PermitProgrammersActionIfTurnChangedToPlayer => 모든 프로그래머의 행동 여부가 초기화됨.");
        }
    }

    public TurnState Turn
    {
        get
        {
            return currentTurn;
        }
        set
        {
            CommonLogger.LogFormat("UnitManager::Turn => 턴이 '{0}'으로 바뀌려 함.", value);

            currentTurn = value;
            OnTurnChanged(currentTurn);
        }
    }

    public bool IsAbleToAct(Programmer programmer)
    {
        if (programmerActingDictionary.ContainsKey(programmer))
        {
            return !programmerActingDictionary[programmer];
        }

        DebugLogger.LogWarning("UnitManager::IsAbleToAct => 전달된 프로그래머가 현재 사전에 존재하지 않습니다.");
        return false;
    }

    public void Dispose()
    {
        programmerActingDictionary.Clear();
    }

    public void DisposeRegisteredEvents()
    {
        OnTurnChanged = delegate { };

        foreach (var programmer in programmerActingDictionary.Keys)
        {
            programmer.DisposeRegisteredEvents();
        }

        CommonLogger.Log("UnitManager::DisposeRegisteredEvents => 이벤트 Disposing 완료.");
    }
}
