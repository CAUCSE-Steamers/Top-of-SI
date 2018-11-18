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

        description.text = skill.ToString();
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
