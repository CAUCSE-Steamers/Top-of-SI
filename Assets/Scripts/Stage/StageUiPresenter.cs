using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using Model;

public class StageUiPresenter : MonoBehaviour
{
    [SerializeField]
    private Animator stateAnimator;
    [SerializeField]
    private ObjectInformationPresenter objectInformationPresenter;
    [SerializeField]
    private StageInformationPresenter stageInformationPresenter;
    [SerializeField]
    private StageNoticeUiPresenter stageNoticeUiPresenter;
    [SerializeField]
    private FinishPresenter victoryPresenter;
    [SerializeField]
    private FinishPresenter failurePresenter;
    [SerializeField]
    private GameObject blockingUi;
    [SerializeField]
    private ModalPresenter modalPresenter;
    [SerializeField]
    private AlternativeProgrammerSelectPresenter alternativeProgrammerPresenter;

    public void SetNewProgrammer(Programmer newProgrammer)
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();
        idleState.AddProgrammerEvent(newProgrammer);
        
        newProgrammer.OnActionStarted += () =>
        {
            objectInformationPresenter.ResetInformationUi();
            SetBlockUiState(true);
        };

        newProgrammer.OnActionFinished += () => SetBlockUiState(false);
    }

    private void Start()
    {
        objectInformationPresenter.ResetInformationUi();

        StageManager.Instance.RefreshPresenter(this);

        AddEnterEvent<IdleState>(StartUiSynchronizing);

        AddEnterEvent<IdleState>(() =>
        {
            var idleState = stateAnimator.GetBehaviour<IdleState>();
            idleState.OnSelected += objectInformationPresenter.SetObjectInformation;
        });

        var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
        moveState.OnMovingStarted += () => objectInformationPresenter.SetEffectActiveState(false);

        foreach (var programmer in StageManager.Instance.Unit.Programmers)
        {
            programmer.OnActionStarted += () =>
            {
                objectInformationPresenter.ResetInformationUi();
                SetBlockUiState(true);
            };

            programmer.OnActionFinished += () => SetBlockUiState(false);
        }

        objectInformationPresenter.OnSkillInvoked += InvokeSkill;
    }

    public void SetBlockUiState(bool newState)
    {
        blockingUi.SetActive(newState);
    }

    private void StartUiSynchronizing()
    {
        RemoveEnterEvent<IdleState>(StartUiSynchronizing);

        stageInformationPresenter.StartSynchronizing();
    }

    public void GoToSelectingMoveCell()
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();

        if (idleState.IsProgrammerSelected)
        {
            var programmer = idleState.SelectedObject.GetComponent<Programmer>();
            var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
            moveState.SetSelectedProgrammer(programmer);

            idleState.TransitionToMoveState();

            objectInformationPresenter.RenderCancelMove();
        }
    }

    public void SelectToVacation()
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();

        if (idleState.IsProgrammerSelected)
        {
            var programmer = idleState.SelectedObject.GetComponent<Programmer>();
            var vacationState = stateAnimator.GetBehaviour<VacationStartState>();

            idleState.TransitionToVacationState();
            vacationState.SetSelectedProgrammer(programmer);

            objectInformationPresenter.RenderStartVacation();
        }
    }

    public void ReturnFromVacationToIdle(bool confirmingVacation)
    {
        var vacationState = stateAnimator.GetBehaviour<VacationStartState>();

        if (confirmingVacation)
        {
            ChangeProgrammerAlphaColor(vacationState.SelectedProgrammer, 0.4f);
            vacationState.ConfirmVacation();
            objectInformationPresenter.ResetInformationUi();
        }
        else
        {
            var idleState = stateAnimator.GetBehaviour<IdleState>();
            idleState.ReserveSetSelectedObject(vacationState.SelectedProgrammer.gameObject);

            objectInformationPresenter.RenderSkillPanel(vacationState.SelectedProgrammer);
        }

        vacationState.TransitionToIdle();
    }

    public void CancelMove()
    {
        var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
        moveState.DisableCellEffect(gameObject);

        var idleState = stateAnimator.GetBehaviour<IdleState>();
        idleState.ReserveSetSelectedObject(moveState.SelectedProgrammer.gameObject);

        moveState.TransitionToIdle();

        objectInformationPresenter.RenderSkillPanel(moveState.SelectedProgrammer);
    }

    public void TogglePause()
    {
        ToggleState
        (
            StateParameter.Pause,
            onTrueValue: stateAnimator.GetBehaviour<PauseState>().TransitionToIdle,
            onFalseValue: stateAnimator.GetBehaviour<IdleState>().TransitionToPauseState
        );
    }

    public void ToggleSetting()
    {
        ToggleState
        (
            StateParameter.Setting,
            onTrueValue: stateAnimator.GetBehaviour<SettingState>().TransitionToIdle,
            onFalseValue: stateAnimator.GetBehaviour<IdleState>().TransitionToSettingState
        );
    }

    public void GivoUpStage()
    {
        ToggleState
        (
            StateParameter.Pause,
            onTrueValue: stateAnimator.GetBehaviour<PauseState>().TransitionToIdle,
            onFalseValue: delegate { }
        );

        TransitionToFailure("프로젝트 완수를 포기하셨습니다.");
    }

    public void TransitionToFailure(params string[] messages)
    {
        failurePresenter.Present(messages);
        stateAnimator.GetBehaviour<IdleState>().TransitionToFailureState();
    }

    public void TransitionToVictory(params string[] messages)
    {
        victoryPresenter.Present(messages);
        stateAnimator.GetBehaviour<IdleState>().TransitionToVictoryState();
    }

    private void ToggleState(StateParameter stateParameter, Action onTrueValue, Action onFalseValue)
    {
        bool value = stateAnimator.GetStateBool(stateParameter);
        if (value)
        {
            onTrueValue();
        }
        else
        {
            onFalseValue();
        }
    }

    public void AddEnterEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnEntered += action;
        }
    }

    public void RemoveEnterEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnEntered -= action;
        }
    }

    public void AddUpdateEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnUpdated += action;
        }
    }

    public void AddExitEvent<T>(Action action) where T : DispatchableState
    {
        foreach (var behaviour in stateAnimator.GetBehaviours<T>())
        {
            behaviour.OnExit += action;
        }
    }

    public void InvokeSkill(ActiveSkill skill)
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();
        var currentSelectedProgrammer = idleState.SelectedObject.GetComponent<Programmer>();

        if (skill is IEffectProducible)
        {
            var effectObject = (skill as IEffectProducible).MakeEffect(currentSelectedProgrammer.transform);

            currentSelectedProgrammer.OnSkillEnded += () =>
            {
                Destroy(effectObject);
            };
        }

        if (skill is ISoundProducible)
        {
            var effectSoundClip = (skill as ISoundProducible).EffectSound;

            var audioSource = SoundManager.Instance.FetchAvailableSource();
            audioSource.PlayOneShot(effectSoundClip);
        }

        var boss = StageManager.Instance.Unit.Boss;

        currentSelectedProgrammer.UseSkill();
        currentSelectedProgrammer.SpendSkillCost(skill.Cost);

        skill.OnSkillMissed += HandleMissedSkill;
        skill.ApplySkill(boss, boss.Ability.ProjType, boss.Ability.Techtype);
        skill.OnSkillMissed -= HandleMissedSkill;

        objectInformationPresenter.ResetInformationUi();
        idleState.ResetSelectedObject();
    }

    public void ActOnVacation(bool isReturning)
    {
        var idleState = stateAnimator.GetBehaviour<IdleState>();
        var currentSelectedProgrammer = idleState.SelectedObject.GetComponent<Programmer>();
        int elapsedDays = StageManager.Instance.Status.ElapsedDays;
        
        if (isReturning)
        {
            currentSelectedProgrammer.ReturnFromVacation(elapsedDays);
            ChangeProgrammerAlphaColor(currentSelectedProgrammer, 1f);
        }

        currentSelectedProgrammer.ActFinish();
        objectInformationPresenter.ResetInformationUi();

        StageManager.Instance.Unit.CheckProgrammerFormation(Vector3.zero);
    }

    public void ChangeProgrammerAlphaColor(Programmer programmer, float alphaValue)
    {
        foreach (var renderer in programmer.GetComponentsInChildren<Renderer>())
        {
            foreach (var material in renderer.materials)
            {
                material.color = new Color(1, 1, 1, alphaValue);
            }
        }
    }

    private void HandleMissedSkill(ActiveSkill activeSkill)
    {
        RenderPlayerText("공격이 빗나갔습니다!");
    }

    public void RenderBossSkillNotice(ProjectSkill skill)
    {
        stageNoticeUiPresenter.RenderBossSkillNotice(skill);
    }

    public void RenderPlayerText(string text)
    {
        stageNoticeUiPresenter.RenderPlayerText(text);
    }

    public void RenderDeathText(IEnumerable<Programmer> deadProgrammers)
    {
        modalPresenter.SetActive(true);

        var names = string.Join(", ", deadProgrammers.Select(programmer => programmer.Status.Name).ToArray());
        var deadBodyText = string.Format("프로그래머 {0}가 업무를 감당하지 못하고 퇴사했습니다!", names);

        modalPresenter.AddClickAction(() => RenderAlternativeProgrammerSelection(deadProgrammers));
        
        modalPresenter.Present("퇴사!", deadBodyText);
    }

    public void RenderAlternativeProgrammerSelection(IEnumerable<Programmer> deadProgrammers)
    {
        modalPresenter.SetActive(false);
        alternativeProgrammerPresenter.gameObject.SetActive(true);
        alternativeProgrammerPresenter.DeadProgrammers = deadProgrammers;
        alternativeProgrammerPresenter.Present(StageManager.Instance.Unit.Programmers);
    }

    public void RenderPaymentText(IEnumerable<Programmer> deadProgrammers)
    {
        modalPresenter.ResetClickAction();

        modalPresenter.AddClickAction(() =>
        {
            modalPresenter.ResetClickAction();
            modalPresenter.SetActive(false);
        });

        var names = string.Join(", ", deadProgrammers.Select(programmer => programmer.Status.Name).ToArray());
        int payment = deadProgrammers.Aggregate(0, (totalPay, programmer) => totalPay + programmer.Status.Cost.Fire);

        LobbyManager.Instance.CurrentPlayer.Money -= payment;
        modalPresenter.Present("퇴직금 지출",
            string.Format("프로그래머 {0}의 퇴직금으로 총 {1} 골드가 소모되었습니다.", names, payment));
    }
}
