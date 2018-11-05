using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Model;

public class LobbyUiPresenter : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text projectTitleText;
    [SerializeField]
    private Text elapsedDayText;
    [SerializeField]
    private Text payText;
    [SerializeField]
    private Button previousProject;
    [SerializeField]
    private Button nextProject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void updateProject(GameStage stage, bool isFirst, bool isLast)
    {
        projectTitleText.text = stage.Title;
        elapsedDayText.text = stage.ElapsedDayLimit.ToString();
        
        if (isFirst)
        {
            previousProject.gameObject.SetActive(false);
        }
        else
        {
            previousProject.gameObject.SetActive(true);
        }

        if (isLast)
        {
            nextProject.gameObject.SetActive(false);
        }
        else
        {
            nextProject.gameObject.SetActive(true);
        }
    }

    public void ChangeToNextProject()
    {

    }
}
