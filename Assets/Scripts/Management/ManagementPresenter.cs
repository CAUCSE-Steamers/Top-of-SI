using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Model;

public class ManagementPresenter : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private ProgrammerListPresenter programmerListPresenter;
    [SerializeField]
    private ManagementInformationPresenter informationPresenter;

    private Player currentPlayer;

    private void Start()
    {
        currentPlayer = LobbyManager.Instance.CurrentPlayer;
        programmerListPresenter.OnCellButtonClicked += (sender, spec) =>
        {
            informationPresenter.Present(spec);
        };

        Present();
        informationPresenter.Disable();
    }

    public void Present()
    {
        moneyText.text = currentPlayer.Money.ToString();
        programmerListPresenter.Present();
        informationPresenter.Refresh();
    }
}
