using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class FinishPresenter : MonoBehaviour
{
    [SerializeField]
    private Transform objectivePanelObject;
    [SerializeField]
    private ObjectiveUiTemplate objectiveTemplate;
    [SerializeField]
    private AudioClip effectSound;

    public void Present(params string[] messages)
    {
        if (effectSound != null)
        {
            SoundManager.Instance.FetchAvailableSource().PlayOneShot(effectSound);
        }

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
        int totalMaintainCost = LobbyManager.Instance.CurrentPlayer.ProgrammerSpecs.Aggregate(0, (acc, spec) => acc + spec.Status.Cost.Pay);
        LobbyManager.Instance.CurrentPlayer.Money -= totalMaintainCost;

        foreach (var message in messages.Union(new List<string> { string.Format("현재 소유하고 있는 프로그래머의 보수로 총 {0} 골드가 지출되었습니다.", totalMaintainCost) }))
        {
            var objectiveUserInterface = Instantiate(objectiveTemplate, objectivePanelObject);
            objectiveUserInterface.Render(message);
        }
    }
}
