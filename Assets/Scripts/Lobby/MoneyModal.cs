using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyModal : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;

    private void OnEnable()
    {
        moneyText.text = string.Format("소유 금액이 부족합니다. (현재 소유 금액 : {0} 골드)", LobbyManager.Instance.CurrentPlayer.Money);
    }
}
