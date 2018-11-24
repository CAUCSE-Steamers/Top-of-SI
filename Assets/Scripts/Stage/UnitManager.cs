using Model;
using Model.Formation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

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
    private AbstractProject boss;
    private TurnState currentTurn;
    private Field stageField;

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

    public IEnumerable<Programmer> NotVacationProgrammers
    {
        get
        {
            return Programmers.Where(programmer => programmer.Status.IsOnVacation == false);
        }
    }


    public AbstractProject Boss
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

    public void SetUnits(IEnumerable<Programmer> programmers, AbstractProject boss, Field stageField)
    {
        CommonLogger.Log("UnitManager::SetUnits => 초기화 시작.");

        if (programmers == null || boss == null || stageField == null)
        {
            DebugLogger.LogError("UnitManager::SetUnits => 주어진 프로그래머, 보스 또는 필드 중 Null이 존재함.");
            throw new ArgumentNullException();
        }

        CurrentAppliedFormation = null;

        OnTurnChanged += RequestBossActionIfTurnChangedToBoss;
        OnTurnChanged += PermitProgrammersActionIfTurnChangedToPlayer;
        OnTurnChanged += ApplyBurfsIfTurnChangedToPlayer;
        OnTurnChanged += DecreaseActiveSkillCooldownIfTurnChangedToPlayer;
        programmerActingDictionary =
            programmers.ToDictionary(keySelector: programmer => programmer,
                                     elementSelector: programmer => false);

        this.boss = boss;
        this.stageField = stageField;
        
        SubscribeToBoss();
        SubscribeToProgrammers();

        Turn = TurnState.Player;
        SetVacationLimitToProgrammers();

        CommonLogger.Log("UnitManager::SetUnits => 초기화 완료.");
    }

    private void SetVacationLimitToProgrammers()
    {
        foreach (var programmer in Programmers)
        {
            programmer.Status.RemainingVacationDay = StageManager.Instance.CurrentStage.ElapsedDayLimit;
        }
    }

    private void ApplyBurfsIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            foreach (var programmer in Programmers)
            {
                programmer.ApplyPersistentStatusBurfs();
            }
        }
    }

    private void DecayProgrammerBurfs()
    {
        CommonLogger.Log("UnitManager::DecayBurfsIfTurnChangedToBoss => 프로그래머의 버프 유지 시간을 감소시킴.");
        foreach (var programmer in Programmers)
        {
            programmer.DecayBurfs();
        }
    }

    private void SubscribeToProgrammers()
    {
        foreach (var programmer in programmerActingDictionary.Keys)
        {
            programmer.OnActionStarted += () =>
            {
                StageManager.Instance.StageField.BlockCellClicking();

                foreach (var prog in Programmers)
                {
                    prog.gameObject.layer = 2;
                }
            };

            programmer.OnActionFinished += () =>
            {
                StageManager.Instance.StageField.UnblockCellClicking();

                foreach (var prog in Programmers)
                {
                    prog.gameObject.layer = Programmer.Layer;
                }

                programmerActingDictionary[programmer] = true;
                CheckProgrammerFormation(Vector3.zero);
                ChangeTurnToBossIfAllProgrammersPerformAction();
            };

            CheckProgrammerFormation(programmer.transform.position);
        }
    }

    public void CheckProgrammerFormation(Vector3 position)
    {
        ResetAppliedFormation();

        foreach (var formation in Formation.formations)
        {
            if (formation.CanApplyFormation())
            {
                CurrentAppliedFormation = formation;

                CommonLogger.LogFormat("UnitManager::CheckProgrammerFormation => 진형 '{0}'가 적용됨.", CurrentAppliedFormation.Name);
                CurrentAppliedFormation.AttachBurfs(NotVacationProgrammers);

                break;
            }
        }
    }

    private void ResetAppliedFormation()
    {
        if (CurrentAppliedFormation != null)
        {
            CurrentAppliedFormation.DetachBurfs();
        }

        CurrentAppliedFormation = null;
    }

    public Formation CurrentAppliedFormation
    {
        get; private set;
    }

    private void ChangeTurnToBossIfAllProgrammersPerformAction()
    {
        if (programmerActingDictionary.Values.All(actingState => actingState) && currentTurn != TurnState.GameEnd)
        {
            CommonLogger.Log("UnitManager::ChangeTurnToBossIfAllProgrammersPerformAction => 모든 프로그래머가 행동을 수행해 턴이 보스로 넘어감.");

            Turn = TurnState.Boss;
        }
        else if (currentTurn == TurnState.GameEnd)
        {
            CommonLogger.Log("UnitManager::ChangeTurnToBossIfAllProgrammersPerformAction => 모든 프로그래머가 행동을 수행했고, 보스가 사망함.");
        }
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
        boss.OnActionFinished += DecayProgrammerBurfs;
        boss.OnActionFinished += () => Turn = TurnState.Player;
        boss.OnDeath += () => Turn = TurnState.GameEnd;
    }

    private void RequestBossActionIfTurnChangedToBoss(TurnState turn)
    {
        if (Turn == TurnState.Boss)
        {
            if (Programmers.Where(programmer => programmer.Status.IsOnVacation).Count() ==
                Programmers.Count())
            {
                CommonLogger.Log("UnitManager::RequestBossActionIfTurnChangedToBoss => 보스에게 행동을 요청하려 했으나, 모든 프로그래머가 휴가 중이므로 취소됨.");

                StageManager.Instance.StageUi.RenderPlayerText("프로젝트가 아무런 행동도 수행하지 않았습니다.");
                boss.InvokeFinished();
                return;
            }

            CommonLogger.Log("UnitManager::RequestBossActionIfTurnChangedToBoss => 보스에게 행동을 요청함.");

            //Decrease Boss Skill Cool
            foreach (ProjectSkill iter in Boss.Ability.ProjectSkills)
            {
                iter.DecreaseCooldown();
            }
            //Decrease Boss Burf Cool and Remove if it's Cool down
            for(int i = 0, delete = 0; i < Boss.Status.Burf.Count; i++)
            {
                Boss.Status.Burf[i - delete].DecreaseTurn();
                if (Boss.Status.Burf[i - delete].Turn < 1)
                {
                    Boss.Status.Burf.RemoveAt(i - delete);
                    delete++;
                }
            }

            if(UnityEngine.Random.Range(0, 12) > 9)
            {
                //MOVE
                StageManager.Instance.MoveBoss();
                StageManager.Instance.StageUi.RenderPlayerText("프로젝트의 방향이 전환되었습니다!");
            }
            else
            {
                //DO Attack or Skill
                var usedSkill = boss.Invoke();
                if (usedSkill is ISoundProducible)
                {
                    var clip = (usedSkill as ISoundProducible).EffectSound;
                    SoundManager.Instance.FetchAvailableSource().PlayOneShot(clip);
                }

                StageManager.Instance.StageUi.RenderBossSkillNotice(usedSkill);

                CommonLogger.LogFormat("UnitManager::RequestBossActionIfTurnChangedToBoss => 보스가 {0} 스킬을 사용함.", usedSkill.Information.Name);

                switch (usedSkill.Information.Type)
                {
                    case ProjectSkillType.SingleAttack:
                        InvokeSkill((ProjectSingleAttackSkill)usedSkill);
                        break;
                    case ProjectSkillType.MultiAttack:
                        InvokeSkill((ProjectMultiAttackSkill)usedSkill);
                        break;
                    case ProjectSkillType.SingleDeburf:
                        InvokeSkill((ProjectSingleDeburfSkill)usedSkill);
                        break;
                    case ProjectSkillType.MultiDeburf:
                        InvokeSkill((ProjectMultiDeburfSkill)usedSkill);
                        break;
                    case ProjectSkillType.Burf:
                        InvokeSkill((ProjectBurfSkill)usedSkill);
                        break;
                }
            }

            boss.InvokeFinished();
        }
    }

    private void ApplyDamage(Programmer programmer, double damage)
    {
        if (programmer.Status.HasBurf<DamageSplashBurf>())
        {
            var burf = programmer.Status.GetBurf<DamageSplashBurf>();
            burf.Accept(damage);
        }
        else if (programmer.Status.HasBurf<DamageSpreadBurf>())
        {
            var burf = programmer.Status.GetBurf<DamageSpreadBurf>();
            burf.Accept(programmer, damage);
        }
        else if (programmer.Status.HasBurf<TargetedDamageSharingBurf>())
        {
            var burf = programmer.Status.GetBurf<TargetedDamageSharingBurf>();
            burf.Accept(programmer, damage);
        }
        else
        {
            programmer.Hurt((int) damage);
        }
    }

    private void InvokeSkill(ProjectSingleAttackSkill skill)
    {
        int randomIndex = Random.Range(0, Programmers.Count());
        var programmer = NotVacationProgrammers.ElementAt(randomIndex);

        ApplyDamage(programmer, skill.Damage);
        return;
    }

    private void InvokeSkill(ProjectMultiAttackSkill skill)
    {
        foreach (var programmer in NotVacationProgrammers)
        {
            ApplyDamage(programmer, skill.Damage);
            continue;
        }
    }

    private void InvokeSkill(ProjectSingleDeburfSkill skill)
    {
        Programmer targetProgrammer = NotVacationProgrammers.ToList()[(int)(UnityEngine.Random.Range(0, Programmers.Count()))];
        DeburfType blockMove = targetProgrammer.Deburf(skill.Deburf);
        if((blockMove & DeburfType.DisableMovement) == DeburfType.DisableMovement)
        {
            //TODO : let programmer can't move
        }
    }
    private void InvokeSkill(ProjectMultiDeburfSkill skill)
    {
        DeburfType outOfProgrammer = DeburfType.None;
        foreach (var programmer in NotVacationProgrammers)
        {
            outOfProgrammer = (outOfProgrammer | programmer.Deburf(skill.Deburf));
        }
        if ((outOfProgrammer & DeburfType.ShortenDeadLine) == outOfProgrammer)
        {
            StageManager.Instance.Status.setDayLimit(skill.Deburf[0].Factor);
        }
    }
    private void InvokeSkill(ProjectBurfSkill skill)
    {
        Boss.Burf(skill.Burf);
    }

    private void PermitProgrammersActionIfTurnChangedToPlayer(TurnState turn)
    {
        if (turn == TurnState.Player)
        {
            foreach (var programmer in programmerActingDictionary.Keys.ToArray())
            {
                programmerActingDictionary[programmer] = false;
            }
            
            //Burf Turn Decrease and Remove expired Burf
            foreach (var iter in Programmers)
            {
                for (int i = 0, delete = 0; i < iter.Status.Deburf.Count; i++)
                {
                    iter.Status.Deburf[i - delete].DecreaseTurn();
                    if(iter.Status.Deburf[i - delete].Turn < 1)
                    {
                        iter.Status.Deburf.RemoveAt(i - delete);
                        delete++;
                    }
                }
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

        CommonLogger.Log("UnitManager::DisposeRegisteredEvents => 이벤트 Disposing 완료.");
    }
}
