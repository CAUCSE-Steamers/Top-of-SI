using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
        actionBlockingObject.SetActive(StageManager.Instance.Unit.IsAbleToAct(programmer) == false);

        foreach (var activeSkill in programmer.Ability.AcquiredActiveSkills)
        {
            var skillPresenter = Instantiate(skillPresenterTemplate, skillPanelObject);

            skillPresenter.RenderSkill(activeSkill);
            skillPresenter.ActivationButton.onClick.AddListener(() => OnSkillInvoked(activeSkill));
        }
    }
}
