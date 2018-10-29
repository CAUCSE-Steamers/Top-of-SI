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
        selectedEffectObject.SetActive(false);

        foreach (var skillPresenter in skillPanelObject.GetComponentsInChildren<SkillPresenter>())
        {
            Destroy(skillPresenter.gameObject);
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
        var programmerPosition = programmer.transform.position;

        selectedEffectObject.SetActive(true);
        selectedEffectObject.transform.position =
            new Vector3(programmerPosition.x, programmerPosition.y + 0.5f, programmerPosition.z);

        foreach (var activeSkill in programmer.Ability.AcquiredActiveSkills)
        {
            var skillPresenter = Instantiate(skillPresenterTemplate, skillPanelObject);

            skillPresenter.RenderSkill(activeSkill);
            skillPresenter.ActivationButton.onClick.AddListener(() => OnSkillInvoked(activeSkill));
        }
    }
}
