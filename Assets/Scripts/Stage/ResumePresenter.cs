using UnityEngine;
using System.Collections.Generic;
using System;

public class ResumePresenter : MonoBehaviour
{
    [SerializeField]
    private Transform objectivePanelObject;
    [SerializeField]
    private ObjectiveUiTemplate objectiveTemplate;

    private void OnEnable()
    {
        RemoveOldObjectives();
        RenderObjectives();
    }

    private void RemoveOldObjectives()
    {
        foreach (var objective in objectivePanelObject.GetComponentsInChildren<ObjectiveUiTemplate>())
        {
            Destroy(objective.gameObject);
        }
    }

    private void RenderObjectives()
    {
        StageManager manager = StageManager.Instance;

        foreach (var objective in manager.CurrentStage.Objectives)
        {
            var objectiveUserInterface = Instantiate(objectiveTemplate, objectivePanelObject);
            objectiveUserInterface.Render(objective);
        }
    }
}
