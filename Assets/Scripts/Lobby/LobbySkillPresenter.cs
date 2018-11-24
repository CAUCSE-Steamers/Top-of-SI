using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Model;

public class LobbySkillPresenter : MonoBehaviour
{
    public event Action<GameObject> OnChangeProject = delegate { };

    [SerializeField]
    private GameObject recommendedSkillPrefab;
    [SerializeField]
    private Transform recommendedSkillListPanel;

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Present()
    {
        RemoveRecommendedSkill();
        ConstructRecommendedSkill();
    }

    public void ConstructRecommendedSkill()
    {
        var currentStage = LobbyManager.Instance.SelectedStage;
        var skill = currentStage.Boss.Ability.ProjType;

        var createdSkill = Instantiate(recommendedSkillPrefab, recommendedSkillListPanel);
        var description = createdSkill.GetComponentInChildren<Text>();

        if (skill == ProjectType.None)
        {
            description.text = "특별한 기술이 필요하지 않습니다.";
        }
        else
        {
            description.text = string.Format("{0} 분야에 최적화된 기술이 필요합니다.", skill.ToString());
        }
    }

    private void RemoveRecommendedSkill()
    {
        foreach (var skill in recommendedSkillListPanel.GetComponentsInChildren<Transform>())
        {
            if (skill != recommendedSkillListPanel)
            {
                Destroy(skill.gameObject);
            }
        }
    }
}
