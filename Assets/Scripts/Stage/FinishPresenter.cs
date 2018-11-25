using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class FinishPresenter : MonoBehaviour
{
    [SerializeField]
    private Transform objectivePanelObject;
    [SerializeField]
    private ObjectiveUiTemplate objectiveTemplate;

    public void Present(params string[] messages)
    {
        RemoveOldObjectives();
        RenderMessages(messages);
    }

    private void RemoveOldObjectives()
    {
        foreach (var objective in objectivePanelObject.GetComponentsInChildren<ObjectiveUiTemplate>())
        {
            Destroy(objective.gameObject);
        }
    }

    private void RenderMessages(IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            var objectiveUserInterface = Instantiate(objectiveTemplate, objectivePanelObject);
            objectiveUserInterface.Render(message);
        }
    }
}
