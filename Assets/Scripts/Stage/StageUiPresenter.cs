using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageUiPresenter : MonoBehaviour
{
    private const string ElapsedDayFormat = "{0}일 째";

    [SerializeField]
    private Text elapsedDayText;
    [SerializeField]
    private Text bossHealthText;
    [SerializeField]
    private Slider bossHealthSlider;

    private StageStatusManager statusManager;
    private UnitManager unitManager;

    private GameObject currentSelectedObject;
    public void SetPresenter(StageStatusManager statusManager, UnitManager unitManager)
    {
        CommonLogger.Log("StageUiPresenter::SetPresenter => 초기화 시작.");

        this.statusManager = statusManager;
        this.unitManager = unitManager;

        statusManager.OnElapsedDayChanged += RefreshElapsedDayUi;
        CommonLogger.Log("StageUiPresenter::SetPresenter => 초기화 완료.");
    }

    private void RefreshElapsedDayUi(int newElapsedDay)
    {
        elapsedDayText.text = string.Format(ElapsedDayFormat, newElapsedDay.ToString());
    }
}
