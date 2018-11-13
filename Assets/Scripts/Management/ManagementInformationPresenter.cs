using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Model;
using System.Linq;

public class ManagementInformationPresenter : MonoBehaviour
{
    [SerializeField]
    private Text[] informationTexts;
    [SerializeField]
    private Image portraitImage;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private GameObject skillCellTemplate;
    [SerializeField]
    private Transform skillCellPanelTransform;
    [SerializeField]
    private GameObject rootObject;

    public void Present(ProgrammerSpec spec)
    {
        rootObject.SetActive(true);

        PresentProgrammerInformation(spec);
        PresentActiveSkill(spec.Ability);
    }

    public void Disable()
    {
        rootObject.SetActive(false);
    }

    private void PresentProgrammerInformation(ProgrammerSpec spec)
    {
        nameText.text = spec.Status.Name;
        portraitImage.sprite = ResourceLoadUtility.LoadPortrait(spec.Status.PortraitName);

        informationTexts[0].text = string.Format("정신력 : {0} / {1}", spec.Status.Health, spec.Status.FullHealth);
        informationTexts[1].text = string.Format("리더쉽 : {0}", spec.Status.Leadership);
        informationTexts[2].text = string.Format("사교성 : {0}", spec.Status.Sociality);
    }

    private void PresentActiveSkill(ProgrammerAbility ability)
    {
        RemoveExistingSkillCell();

        foreach (var activeSkill in ability.AcquiredActiveSkills.Where(skill => skill.Information.Type != SkillType.None)
                                                                .OrderBy(skill => skill.Information.Name))
        {
            var createdCell = Instantiate(skillCellTemplate, skillCellPanelTransform);

            var skillImage = createdCell.GetComponentInChildren<Image>();
            skillImage.sprite = ResourceLoadUtility.LoadIcon(activeSkill.Information.IconName);

            var skillText = createdCell.GetComponentInChildren<Text>();
            skillText.text = string.Format("{0} ({1} / {2})", activeSkill.Information.Name, activeSkill.Information.AcquisitionLevel, activeSkill.Information.MaximumLevel);

            var upgradeButton = createdCell.GetComponentInChildren<Button>();

        }
    }

    private void RemoveExistingSkillCell()
    {
        foreach (var existingCell in skillCellPanelTransform.GetComponentsInChildren<Transform>())
        {
            if (existingCell != skillCellPanelTransform)
            {
                Destroy(existingCell.gameObject);
            }
        }
    }
}
