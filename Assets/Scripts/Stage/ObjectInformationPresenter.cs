using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectInformationPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedEffectObject;

    public void SetObjectInformation(GameObject objectToView)
    {
        selectedEffectObject.SetActive(false);

        var programmerComponent = objectToView.GetComponent<Programmer>();
        if (programmerComponent != null)
        {
            var programmerPosition = programmerComponent.transform.position;

            selectedEffectObject.SetActive(true);
            selectedEffectObject.transform.position =
                new Vector3(programmerPosition.x, programmerPosition.y + 0.5f, programmerPosition.z);
        }
    }

    public void ClearInformationUi()
    {
        selectedEffectObject.SetActive(false);
    }
}
