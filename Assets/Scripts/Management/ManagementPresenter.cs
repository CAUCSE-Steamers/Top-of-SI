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

    private Player currentPlayer;

    private void Start()
    {
        currentPlayer = LobbyManager.Instance.CurrentPlayer;
        Present();
    }

    private void Present()
    {
        moneyText.text = currentPlayer.Money.ToString();
        programmerListPresenter.Present();
    }
}
