using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Model;
using System.Linq;
using System.Collections.Generic;

public class SkillManagementPresenter : MonoBehaviour
{
    [SerializeField]
    private Image skillIconImage;
    [SerializeField]
    private GameObject skillCellTemplate;
    [SerializeField]
    private GameObject blankCellTemplate;
    [SerializeField]
    private Transform activeSkillParentTransform;
    [SerializeField]
    private Transform passiveSkillParentTransform;
    [SerializeField]
    private Transform auxPassiveSkillParentTransform;
    [SerializeField]
    private SkillUpgradePresenter skillUpgradePresenter;
    [SerializeField]
    private UnityEvent onUpgradeClicked;

    public void Present(ActiveSkill activeSkill)
    {
        ClearPanel(activeSkillParentTransform);
        ClearPanel(passiveSkillParentTransform);
        ClearPanel(auxPassiveSkillParentTransform);
        
        skillIconImage.sprite = ResourceLoadUtility.LoadIcon(activeSkill.Information.IconName);

        ConstructActivePanel(activeSkill);
        ConstructPassivePanel(activeSkill.AuxiliaryPassiveSkills);
    }

    private void ClearPanel(Transform parentTransform)
    {
        foreach (var existingCell in parentTransform.GetComponentsInChildren<Transform>())
        {
            if (existingCell != parentTransform)
            {
                Destroy(existingCell.gameObject);
            }
        }
    }

    private void ConstructActivePanel(ActiveSkill activeSkill)
    {
        var createdCell = CreateSkillCell(activeSkillParentTransform, activeSkill.Information);
        var buttonComponent = createdCell.GetComponentInChildren<Button>();
        buttonComponent.onClick.AddListener(() =>
        {
            skillUpgradePresenter.Present(activeSkill);
            onUpgradeClicked.Invoke();
        });
    }

    private void ConstructPassivePanel(IEnumerable<PassiveSkill> passiveSkills)
    {
        foreach (var passiveSkill in passiveSkills)
        {
            var createdCell = CreateSkillCell(passiveSkillParentTransform, passiveSkill.Information);
            var buttonComponent = createdCell.GetComponentInChildren<Button>();
            buttonComponent.onClick.AddListener(() =>
            {
                skillUpgradePresenter.Present(passiveSkill);
                onUpgradeClicked.Invoke();
            });

            CreateBlankCell(passiveSkillParentTransform, passiveSkill.AuxiliaryPassiveSkills.Count() - 1);
            ConstructAuxPassivePanel(passiveSkill.AuxiliaryPassiveSkills);
        }
    }

    private void ConstructAuxPassivePanel(IEnumerable<PassiveSkill> passiveSkills)
    {
        int auxPassiveCount = passiveSkills.Count();

        if (auxPassiveCount == 0)
        {
            CreateBlankCell(auxPassiveSkillParentTransform, 1);
        }
        else
        {
            foreach (var passiveSkill in passiveSkills)
            {
                CreateSkillCell(auxPassiveSkillParentTransform, passiveSkill.Information);
            }
        }
    }

    private void CreateBlankCell(Transform parentTransform, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var createdCell = Instantiate(blankCellTemplate, parentTransform);
        }
    }

    private GameObject CreateSkillCell(Transform parentTransform, SkillBasicInformation information)
    {
        var createdCell = Instantiate(skillCellTemplate, parentTransform);
        
        if (information.LearnEnabled == false)
        {
            var textComponent = createdCell.GetComponentInChildren<Text>();
            var buttonComponent = createdCell.GetComponentInChildren<Button>();
            var imageComponent = createdCell.GetComponentInChildren<Image>();

            textComponent.color = Color.magenta;
            textComponent.text = "Locked";

            buttonComponent.interactable = false;
            
            imageComponent.color = new Color(0, 0, 0, 1f);
        }
        else
        {
            createdCell.GetComponentInChildren<Text>().text = string.Format("{0} ({1} / {2})", information.Name, information.AcquisitionLevel, information.MaximumLevel);
        }

        return createdCell;
    }
}
