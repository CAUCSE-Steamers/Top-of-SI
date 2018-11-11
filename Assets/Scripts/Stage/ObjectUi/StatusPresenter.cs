using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Model;
using System.Linq;

public class StatusPresenter : MonoBehaviour
{
    [SerializeField]
    private ImageTextPair[] statusUiList;
    [SerializeField]
    private ImageTextPair formationUi;
    [SerializeField]
    private ImageTextPair burfUi;
    [SerializeField]
    private ImageTextPair deburfUi;

    public void Present(ProgrammerStatus status)
    {
        PresentFormationStatus();

        SetTextsActiveState(true);
        statusUiList[0].SetText(string.Format("정신력 : {0} / {1}", status.Health, status.FullHealth));
        statusUiList[1].SetText(string.Format("리더쉽 : {0}", status.Leadership));
        statusUiList[2].SetText(string.Format("사교성 : {0}", status.Sociality));

        burfUi.SetText(status.Burfs.Count().ToString());
        deburfUi.SetText(status.Burfs.Count().ToString());
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

        burfUi.SetActiveState(true);
        deburfUi.SetActiveState(true);
    }

    public void Disable()
    {
        SetTextsActiveState(false);
        formationUi.SetActiveState(false);
        burfUi.SetActiveState(false);
        deburfUi.SetActiveState(false);
    }
}
