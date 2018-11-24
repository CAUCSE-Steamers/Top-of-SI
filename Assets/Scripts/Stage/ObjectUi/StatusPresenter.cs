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
    private BurfPresenter burfUi;
    [SerializeField]
    private BurfPresenter deburfUi;

    private void Start()
    {
        burfUi.OnPointerEntered += sender =>
        {
            if (Status != null && Status.PositiveBurfs.Count() > 0)
            {
                burfUi.EnableBurfPopup();
            }
        };

        deburfUi.OnPointerEntered += sender =>
        {
            if (Status != null && Status.NegativeBurfs.Count() > 0)
            {
                deburfUi.EnableBurfPopup();
            }
        };

        burfUi.OnPointerExited += sender => burfUi.DisableBurfPopup();
        deburfUi.OnPointerExited += sender => deburfUi.DisableBurfPopup();
    }

    public ProgrammerStatus Status
    {
        get; private set;
    }

    public void Present(ProgrammerStatus status)
    {
        Status = status;

        burfUi.Present(status.PositiveBurfs);
        deburfUi.Present(status.NegativeBurfs);

        PresentFormationStatus();

        SetTextsActiveState(true);
        statusUiList[0].SetText(string.Format("정신력 : {0} / {1}", status.Health, status.FullHealth));
        statusUiList[1].SetText(string.Format("리더쉽 : {0}", status.Leadership));
        statusUiList[2].SetText(string.Format("사교성 : {0}", status.Sociality));
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

        burfUi.Disable();
        deburfUi.Disable();
    }
}
