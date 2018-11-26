using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class ModalPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject modalRootObject;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text bodyText;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button okButton;
    [SerializeField]
    private AudioClip clip;
    
    public void SetActive(bool activeState)
    {
        modalRootObject.SetActive(activeState);
    }

    public void Present(string title, string body)
    {
        if (clip != null)
        {
            SoundManager.Instance.FetchAvailableSource().PlayOneShot(clip);
        }

        titleText.text = title;
        bodyText.text = body;
    }

    public void AddClickAction(UnityAction clickAction)
    {
        closeButton.onClick.AddListener(clickAction);
        okButton.onClick.AddListener(clickAction);
    }

    public void ResetClickAction()
    {
        closeButton.onClick.RemoveAllListeners();
        okButton.onClick.RemoveAllListeners();
    }
}
