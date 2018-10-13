using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Model;

public class UnitManager : MonoBehaviour, IEventDisposable
{
    public readonly static IEnumerable<Vector2Int> ProgrammerMovableIndices = new List<Vector2Int>
    {
        new Vector2Int(0, -1),
        new Vector2Int(0, -2),
        new Vector2Int(0, 1),
        new Vector2Int(0, 2),
        new Vector2Int(-1, 0),
        new Vector2Int(-2, 0),
        new Vector2Int(1, 0),
        new Vector2Int(2, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1)
    }.AsReadOnly();

    public event Action<TurnState> OnTurnChanged = delegate { };

    // Programmer (Key) => Programmer has performed any action? (Value)
    private IDictionary<Programmer, bool> programmerActingDictionary;
    private Programmer boss; // TODO : Convert to boss
    private TurnState currentTurn;
    private Field stageField;

    public void ResetCells()
    {
        foreach (var cell in stageField.Cells)
        {
            cell.SetEffectActiveState(false);
        }
    }

    public IEnumerable<Programmer> Programmers
    {
        get
        {
            if (programmerActingDictionary == null)
            {
                DebugLogger.LogError("UnitManager::Programmer => 아직 설정된 프로그래머가 없습니다.");
                throw new NullReferenceException();
            }

            return programmerActingDictionary.Keys;
        }
    }

    // TODO: Convert to boss
    public Programmer Boss
    {
        get
        {
            if (boss == null)
            {
                DebugLogger.LogError("UnitManager::Boss => 아직 설정된 보스가 없습니다.");
            }

            return boss;
        }
    }

    public void SetUnits(IEnumerable<Programmer> programmers, Programmer boss, Field stageField)
    {
        CommonLogger.Log("UnitManager::SetUnits => 초기화 시작.");

        if (programmers == null || boss == null || stageField == null)
        {
            DebugLogger.LogError("UnitManager::SetUnits => 주어진 프로그래머, 보스 또는 필드 중 Null이 존재함.");
            throw new ArgumentNullException();
        }

        OnTurnChanged += RequestBossActionIfTurnChangedToBoss;
        OnTurnChanged += PermitProgrammersActionIfTurnChangedToPlayer;
        OnTurnChanged += DecreaseActiveSkillCooldownIfTurnChangedToPlayer;
        programmerActingDictionary = 
            programmers.ToDictionary(keySelector: programmer => programmer,
                                     elementSelector: programmer => false);

        this.boss = boss;
        this.stageField = stageField;

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

            Register(programmer);
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

    // TODO: Delete
    public Programmer CurrentSelectedProgrammer;
    private void Register(Programmer programmer)
    {
        programmer.OnMouseClicked += () =>
        {
            CurrentSelectedProgrammer = programmer;
        };
    }

    // TODO: Delete
    public void TempSkillUse()
    {
        var skill = CurrentSelectedProgrammer.Ability.AcquiredActiveSkills.First();

        if (skill is IEffectProducible)
        {
            var effectObject = (skill as IEffectProducible).MakeEffect(CurrentSelectedProgrammer.transform);

            CurrentSelectedProgrammer.OnSkillEnded += () =>
            {
                Destroy(effectObject);
            };
        }

        CurrentSelectedProgrammer.UseSkill();
        skill.ApplySkill(boss, ProjectType.Application, RequiredTechType.Web);
    }

    public IEnumerable<Cell> CurrentMovableCellFor(Programmer programmer)
    {
        var currentProgrammerIndexInField = stageField.VectorToIndices(programmer.transform.position);

        var noObjectContainingCells = stageField.FetchObjectNotContainingCells();
        var movableCells = noObjectContainingCells.Where(cell => ProgrammerMovableIndices.Contains(cell.PositionInField - currentProgrammerIndexInField));

        var blockedMovableCells = from cell in movableCells
                                  let difference = cell.PositionInField - currentProgrammerIndexInField
                                  where difference.sqrMagnitude >= 4
                                  let halfMovedIndex = new Vector2Int(currentProgrammerIndexInField.x + (difference.x / 2),
                                                                      currentProgrammerIndexInField.y + (difference.y / 2))
                                  where stageField.GetCell(halfMovedIndex.x, halfMovedIndex.y).HasObjectOnCell()
                                  select cell;

        return movableCells.Except(blockedMovableCells);
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

    private void DecreaseActiveSkillCooldownIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            foreach (var activeSkill in Programmers.SelectMany(programmer => programmer.Ability.AcquiredActiveSkills))
            {
                if (activeSkill is ICooldownRequired)
                {
                    (activeSkill as ICooldownRequired).DecreaseCooldown();
                }

                foreach (var passiveSkill in activeSkill.FlattenContainingPassiveSkills()
                                                        .Where(passiveSkill => passiveSkill is ICooldownRequired)
                                                        .Select(passiveSkill => passiveSkill as ICooldownRequired))
                {
                    passiveSkill.DecreaseCooldown();    
                }
            }
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
