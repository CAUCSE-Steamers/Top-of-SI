using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
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

    private void Start()
    {
        AddEnterEvent<IdleState>(StartUiSynchronizing);

        AddEnterEvent<IdleState>(() =>
        {
            var idleState = stateAnimator.GetBehaviour<IdleState>();
            objectInformationPresenter.ResetInformationUi();
            idleState.OnSelected += objectInformationPresenter.SetObjectInformation;
        });

        objectInformationPresenter.OnSkillInvoked += InvokeSkill;
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
        }
    }

    public void ReturnFromVacationToIdle(bool confirmingVacation)
    {
        var vacationState = stateAnimator.GetBehaviour<VacationStartState>();

        if (confirmingVacation)
        {
            vacationState.ConfirmVacation();
        }

        vacationState.TransitionToIdle();
    }

    public void CancelMove()
    {
        var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
        moveState.DisableCellEffect(gameObject);
        moveState.TransitionToIdle();
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
        stateAnimator.GetBehaviour<IdleState>().TransitionToFailureState();
    }

    public void TransitionToFailure()
    {
        stateAnimator.GetBehaviour<IdleState>().TransitionToFailureState();
    }

    public void TransitionToVictory()
    {
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

        skill.OnSkillMissed += HandleMissedSkill;
        skill.ApplySkill(boss, boss.Ability.ProjType, boss.Ability.Techtype);
        skill.OnSkillMissed -= HandleMissedSkill;
        objectInformationPresenter.ResetInformationUi();
        idleState.ResetSelectedObject();
    }

    private void HandleMissedSkill(ActiveSkill activeSkill)
    {
        RenderBossText("공격이 빗나갔습니다!");
    }

    public void RenderBossSkillNotice(ProjectSkill skill)
    {
        stageNoticeUiPresenter.RenderBossSkillNotice(skill);
    }

    public void RenderBossText(string text)
    {
        stageNoticeUiPresenter.RenderBossText(text);
    }
}
