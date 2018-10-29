using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Model;

public class StatusPresenter : MonoBehaviour
{
    [SerializeField]
    private ImageTextPair[] statusUiList;

    public void Present(ProgrammerStatus status)
    {
        SetTextsActiveState(true);

        statusUiList[0].SetText(string.Format("정신력 : {0}", status.Health));
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
    }
}
