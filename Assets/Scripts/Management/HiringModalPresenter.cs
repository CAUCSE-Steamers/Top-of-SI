using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Model;
using UnityEngine.UI;

public class HiringModalPresenter : MonoBehaviour
{
    [SerializeField]
    private Text informationText;
    [SerializeField]
    private UnityEvent OnMoneyNotEnough;
    [SerializeField]
    private UnityEvent OnHiringConfirmed;

    public void Present()
    {
        var newProgrammerSpec = new ProgrammerSpec();

        informationText.text = string.Format("{0} 골드를 사용하여 새로운 프로그래머를 고용합니다.", newProgrammerSpec.Status.Cost.Hire);
    }

    public void ConfirmHire()
    {
        var newProgrammerSpec = new ProgrammerSpec();

        if (LobbyManager.Instance.CurrentPlayer.Money < newProgrammerSpec.Status.Cost.Hire)
        {
            OnMoneyNotEnough.Invoke();
        }
        else
        {
            OnHiringConfirmed.Invoke();
        }
    }
}
