using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Model;

public class HiringSelectPresenter : MonoBehaviour
{
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private Image portraitImage;
    [SerializeField]
    private Button previousPortraitButton;
    [SerializeField]
    private Button nextPortraitButton;
    [SerializeField]
    private UnityEvent onHiringCompleted;

    private List<Sprite> availablePortraits;
    private int currentPortraitIndex;

    private void Start()
    {
        availablePortraits = new List<Sprite>();
        availablePortraits.AddRange(Resources.LoadAll<Sprite>("Portraits"));
        
        currentPortraitIndex = 0;
    }

    public void Present()
    {
        nameField.text = string.Empty;

        currentPortraitIndex = 0;
        SetPortrait(currentPortraitIndex);
    }

    public void PresentPreviousPortrait()
    {
        SetPortrait(currentPortraitIndex - 1);
    }

    public void PresentNextPortrait()
    {
        SetPortrait(currentPortraitIndex + 1);
    }

    private void SetPortrait(int index)
    {
        portraitImage.sprite = availablePortraits[index];

        previousPortraitButton.gameObject.SetActive(true);
        nextPortraitButton.gameObject.SetActive(true);

        if (index == 0)
        {
            previousPortraitButton.gameObject.SetActive(false);
        }

        if (index == availablePortraits.Count - 1)
        {
            nextPortraitButton.gameObject.SetActive(false);
        }

        currentPortraitIndex = index;
    }

    public void HireProgrammer()
    {
        var newProgrammerSpec = new ProgrammerSpec();
        newProgrammerSpec.Status.Name = nameField.text;
        newProgrammerSpec.Status.PortraitName = availablePortraits[currentPortraitIndex].name;

        LobbyManager.Instance.CurrentPlayer.ProgrammerSpecs.Add(newProgrammerSpec);
        LobbyManager.Instance.CurrentPlayer.Money -= newProgrammerSpec.Status.Cost.Hire;

        onHiringCompleted.Invoke();
    }
}
