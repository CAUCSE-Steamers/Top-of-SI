using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Model;

[Serializable]
public class CooldownPresenter
{
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Text cooldownText;

    public void Present(double defaultCooldown, double remainingCooldown)
    {
        var currentColor = backgroundImage.color;

        float alphaRatio = 0.8f - (0.6f * (float) (defaultCooldown - remainingCooldown) / (float) defaultCooldown);
        backgroundImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaRatio);
        cooldownText.text = remainingCooldown.ToString();

        backgroundImage.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(true);
    }

    public void Disable()
    {
        backgroundImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }
}

public class SkillPresenter : MonoBehaviour
{
    [SerializeField]
    private CooldownPresenter cooldownPresenter;
    [SerializeField]
    private Button skillActivateButton;

    public Button ActivationButton
    {
        get
        {
            return skillActivateButton;
        }
    }

    public void RenderSkill(ActiveSkill activeSkill)
    {
        skillActivateButton.GetComponent<Image>().sprite = activeSkill.Information.Image;
        skillActivateButton.interactable = activeSkill.IsTriggerable;

        if (activeSkill.IsTriggerable == false)
        {
            cooldownPresenter.Present(activeSkill.DefaultCooldown, activeSkill.RemainingCooldown);
        }
        else
        {
            cooldownPresenter.Disable();
        }
    }
}
