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

    private void Start()
    {
        AddEnterEvent<IdleState>(StartUiSynchronizing);

        AddEnterEvent<IdleState>(() =>
        {
            var idleState = stateAnimator.GetBehaviour<IdleState>();
            objectInformationPresenter.ResetInformationUi();
            idleState.OnSelected += objectInformationPresenter.SetObjectInformation;
        });

        var moveState = stateAnimator.GetBehaviour<SelectingMoveState>();
        moveState.OnMovingStarted += () => objectInformationPresenter.SetEffectActiveState(false);

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

        var boss = StageManager.Instance.Unit.Boss;
        currentSelectedProgrammer.UseSkill();
        skill.ApplySkill(boss, ProjectType.Application, RequiredTechType.Web);

        objectInformationPresenter.ResetInformationUi();
        idleState.ResetSelectedObject();
    }
}
