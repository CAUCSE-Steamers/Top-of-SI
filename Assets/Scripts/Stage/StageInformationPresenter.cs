using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageInformationPresenter : MonoBehaviour
{
    private const string ElapsedDayTextFormat = "{0}일 째";
    private const string BossHealthTextFormat = "{0} / {1}";

    [SerializeField]
    private Text projectTitleText;
    [SerializeField]
    private Text elapsedDayText;
    [SerializeField]
    private Text bossNameText;
    [SerializeField]
    private Text bossHealthText;
    [SerializeField]
    private Slider bossHealthSlider;

    private StageManager manager;

    private void Start()
    {
        manager = StageManager.Instance;
    }

    public void StartSynchronizing()
    {
        projectTitleText.text = manager.CurrentStage.Title;

        var boss = manager.Unit.Boss;
        bossNameText.text = boss.Status.Name;

        RefreshElapsedDay(manager.Status.ElapsedDays);
        RefreshBossHealth(boss.Status.Health);

        manager.Status.OnElapsedDayChanged += RefreshElapsedDay;
        manager.Unit.Boss.Status.OnHealthChanged += RefreshBossHealth;
    }

    public void StopSynchronizing()
    {
        manager.Status.OnElapsedDayChanged -= RefreshElapsedDay;
        manager.Unit.Boss.Status.OnHealthChanged -= RefreshBossHealth;
    }

    public void RefreshElapsedDay(int newElapsedDay)
    {
        elapsedDayText.text = string.Format(ElapsedDayTextFormat, newElapsedDay);
    }

    public void RefreshBossHealth(int newBossHealth)
    {
        int fullBossHealth = manager.Unit.Boss.Status.FullHealth;
        float healthRatio = ((float) newBossHealth) / fullBossHealth;

        bossHealthText.text = string.Format(BossHealthTextFormat, newBossHealth, fullBossHealth);
        StartCoroutine(LerpHealthSlider(healthRatio));
    }

    private IEnumerator LerpHealthSlider(float destinationValue)
    {
        float initialValue = bossHealthSlider.value;
        float interpolationRatio = 0.0f;

        while (Mathf.Abs(initialValue - destinationValue) > float.Epsilon)
        {
            interpolationRatio += Time.deltaTime;
            float interpolatedValue = Mathf.Lerp(initialValue, destinationValue, interpolationRatio);

            interpolatedValue = Mathf.Clamp(interpolatedValue, 0.0f, 1.0f);
            bossHealthSlider.value = interpolatedValue;

            yield return new WaitForEndOfFrame();
        }
    }
}
