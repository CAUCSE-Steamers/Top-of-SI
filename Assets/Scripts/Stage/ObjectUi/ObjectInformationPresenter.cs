using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System;
using Model;

public class ObjectInformationPresenter : MonoBehaviour
{
    public event Action<ActiveSkill> OnSkillInvoked = delegate { };

    [SerializeField]
    private Image programmerImage;
    [SerializeField]
    private StatusPresenter statusPresenter;
    [SerializeField]
    private Transform skillPanelObject;
    [SerializeField]
    private SkillPresenter skillPresenterTemplate;
    [SerializeField]
    private GameObject actionBlockingObject;
    [SerializeField]
    private GameObject vacationPanel;
    [SerializeField]
    private GameObject selectedEffectObject;

    public void SetObjectInformation(GameObject objectToView)
    {
        ResetInformationUi();

        if (objectToView != null)
        {
            RenderObject(objectToView);
        }
    }

    public void ResetInformationUi()
    {
        actionBlockingObject.SetActive(false);
        vacationPanel.SetActive(false);

        SetEffectActiveState(false);
        SetDefaultActionState(false);

        foreach (var childButton in skillPanelObject.GetComponentsInChildren<Button>())
        {
            if (childButton.GetComponent<SkillPresenter>() != null)
            {
                Destroy(childButton.gameObject);
            }
        }
    }

    public void SetEffectActiveState(bool newState)
    {
        selectedEffectObject.SetActive(newState);
    }

    private void SetDefaultActionState(bool newState)
    {
        foreach (var childButton in skillPanelObject.GetComponentsInChildren<Button>(true))
        {
            if (childButton.GetComponent<SkillPresenter>() == null)
            {
                childButton.gameObject.SetActive(newState);
            }
        }

        programmerImage.gameObject.SetActive(newState);
        statusPresenter.Disable();
    }

    private void RenderObject(GameObject objectToView)
    {
        var programmerComponent = objectToView.GetComponent<Programmer>();
        if (programmerComponent != null)
        {
            RenderProgrammer(programmerComponent);
        }
    }

    private void RenderProgrammer(Programmer programmer)
    {
        SetDefaultActionState(true);

        RenderStatus(programmer);
        RenderSelectionEffect(programmer);
        RenderSkillPanel(programmer);
    }

    private void RenderStatus(Programmer programmer)
    {
        programmerImage.sprite = ResourceLoadUtility.LoadPortrait(programmer.Status.PortraitName);

        statusPresenter.Present(programmer.Status);
    }

    private void RenderSelectionEffect(Programmer programmer)
    {
        var programmerPosition = programmer.transform.position;

        selectedEffectObject.SetActive(true);
        selectedEffectObject.transform.position =
            new Vector3(programmerPosition.x, programmerPosition.y + 0.5f, programmerPosition.z);
    }

    private void RenderSkillPanel(Programmer programmer)
    {
        skillPanelObject.gameObject.SetActive(false);
        vacationPanel.SetActive(false);

        actionBlockingObject.SetActive(StageManager.Instance.Unit.IsAbleToAct(programmer) == false);

        // TODO: 스테이지 끝나면 휴가에서 강제 송환해야..
        if (programmer.Status.IsOnVacation)
        {
            RenderReturnFromVacation(programmer);
        }
        else
        {
            RenderActiveSkills(programmer);
        }
    }

    private void RenderReturnFromVacation(Programmer programmer)
    {
        vacationPanel.SetActive(true);

        int elapsedDay = StageManager.Instance.Status.ElapsedDays;
        
        var informationTexts = vacationPanel.GetComponentsInChildren<Text>();
        informationTexts[0].text = string.Format("휴가 복귀 (+ 정신력 {0})", programmer.VacationHealthQuantity(elapsedDay));
        informationTexts[1].text = string.Format("휴가 유지 (잔여 휴가 일수 : {0}일)", 2);
    }

    private void RenderActiveSkills(Programmer programmer)
    {
        skillPanelObject.gameObject.SetActive(true);

        foreach (var activeSkill in programmer.Ability.AcquiredActiveSkills
                                                      .Where(skill => skill.Information.AcquisitionLevel > 0))
        {
            var skillPresenter = Instantiate(skillPresenterTemplate, skillPanelObject);

            skillPresenter.RenderSkill(activeSkill);
            skillPresenter.ActivationButton.onClick.AddListener(() => OnSkillInvoked(activeSkill));
        }
    }
}
