using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Model;
using System;

public class StatusPresenter : MonoBehaviour
{
    [SerializeField]
    private ImageTextPair[] statusUiList;
    [SerializeField]
    private ImageTextPair formationUi;

    public void Present(ProgrammerStatus status)
    {
        PresentFormationStatus();

        SetTextsActiveState(true);
        statusUiList[0].SetText(string.Format("정신력 : {0} / {1}", status.Health, status.FullHealth));
    }

    private void PresentFormationStatus()
    {
        var appliedFormation = StageManager.Instance.Unit.CurrentAppliedFormation;
        if (appliedFormation != null)
        {
            formationUi.SetActiveState(true);
            formationUi.SetText(string.Format("{0} 적용중", appliedFormation.Name));
        }
    }

    private void SetTextsActiveState(bool newState)
    {
        foreach (var statusUi in statusUiList)
        {
            statusUi.SetActiveState(newState);
        }
    }

    public void Disable()
    {
        SetTextsActiveState(false);
        formationUi.SetActiveState(false);
    }
}
