using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Model;

public class ObjectInformationPresenter : MonoBehaviour
{
    public event Action<ActiveSkill> OnSkillInvoked = delegate { };

    [SerializeField]
    private Transform skillPanelObject;
    [SerializeField]
    private SkillPresenter skillPresenterTemplate;
    [SerializeField]
    private GameObject actionBlockingObject;

    [SerializeField]
    private GameObject selectedEffectObject;

    private void Start()
    {
    }

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
        selectedEffectObject.SetActive(false);
        actionBlockingObject.SetActive(false);

        SetDefaultActionState(false);

        foreach (var childButton in skillPanelObject.GetComponentsInChildren<Button>())
        {
            if (childButton.GetComponent<SkillPresenter>() != null)
            {
                Destroy(childButton.gameObject);
            }
        }
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

        RenderSelectionEffect(programmer);
        RenderSkillPanel(programmer);
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
