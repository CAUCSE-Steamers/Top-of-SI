using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using Model;

public class SkillUpgradePresenter : MonoBehaviour
{
    [SerializeField]
    private ImageTextPair previousTitlePair;
    [SerializeField]
    private ImageTextPair afterTitlePair;
    [SerializeField]
    private Text previousSkillDescription;
    [SerializeField]
    private Text afterSkillDescription;
    [SerializeField]
    private Text previousMoneyText;
    [SerializeField]
    private Text afterMoneyText;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private UnityEvent onUpgradeCompleted;
    [SerializeField]
    private UnityEvent onMoneyNotEnough;

    public void Present(ActiveSkill activeSkill)
    {
        Present(activeSkill.Information);

        upgradeButton.onClick.AddListener(() =>
        {
            ValidateUpgrade(activeSkill, activeSkill.Information.RequiredUpgradeCost);
        });
    }

    public void Present(PassiveSkill passiveSkill)
    {
        Present(passiveSkill.Information);

        upgradeButton.onClick.AddListener(() =>
        {
            ValidateUpgrade(passiveSkill, passiveSkill.Information.RequiredUpgradeCost);
        });
    }

    private void ValidateUpgrade(ILevelUp levelUpSkill, int cost)
    {
        if (LobbyManager.Instance.CurrentPlayer.Money < cost)
        {
            onMoneyNotEnough.Invoke();
        }
        else
        {
            LobbyManager.Instance.CurrentPlayer.Money -= cost;
            levelUpSkill.LevelUP();

            onUpgradeCompleted.Invoke();
        }
    }

    private void Present(SkillBasicInformation basicInformation)
    {
        upgradeButton.onClick.RemoveAllListeners();

        previousTitlePair.SetText(string.Format("{0} (Lv. {1})", basicInformation.Name, basicInformation.AcquisitionLevel));
        previousTitlePair.SetSprite(basicInformation.Image);
        previousSkillDescription.text = basicInformation.DescriptionFunc(basicInformation.AcquisitionLevel);
        previousMoneyText.text = LobbyManager.Instance.CurrentPlayer.Money.ToString();

        // TODO: 만렙이라 강화할 게 없다!
        afterTitlePair.SetSprite(basicInformation.Image);
        afterTitlePair.SetText(string.Format("{0} (Lv. {1})", basicInformation.Name, basicInformation.AcquisitionLevel + 1));
        afterSkillDescription.text = basicInformation.DescriptionFunc(basicInformation.AcquisitionLevel + 1);
        afterMoneyText.text = (LobbyManager.Instance.CurrentPlayer.Money - basicInformation.RequiredUpgradeCost).ToString();
    }
}
