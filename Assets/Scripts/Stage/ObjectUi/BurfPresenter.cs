using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Model;

public class BurfPresenter : PointEventTrigger
{
    [SerializeField]
    private ImageTextPair burfUi;
    [SerializeField]
    private GameObject totalPanelObject;
    [SerializeField]
    private GameObject burfItemTemplate;
    [SerializeField]
    private Transform burfParentTransform;
    [SerializeField]
    private ScrollRect listScrollRect;

    public IEnumerable<IBurf> Burfs
    {
        get; private set;
    }

    public void Present(IEnumerable<IBurf> burfs)
    {
        Burfs = burfs;

        burfUi.SetActiveState(true);
        burfUi.SetText(Burfs.Count().ToString());
    }

    public void EnableBurfPopup()
    {
        if (Burfs == null)
        {
            return;
        }
        
        DestroyExistingItem();

        foreach (var burf in Burfs)
        {
            var createdItem = Instantiate(burfItemTemplate, burfParentTransform);
            var imageComponent = createdItem.transform.GetComponentInChildren<Image>();
            var textComponent = createdItem.transform.GetComponentsInChildren<Text>();

            imageComponent.sprite = ResourceLoadUtility.LoadIcon(burf.IconName);

            textComponent[0].text = burf.RemainingTurn.ToString();
            textComponent[1].text = burf.Description;
        }

        totalPanelObject.SetActive(true);
    }

    public void DisableBurfPopup()
    {
        totalPanelObject.SetActive(false);
    }

    private void DestroyExistingItem()
    {
        foreach (var existingItem in burfParentTransform.GetComponentsInChildren<Transform>())
        {
            if (existingItem.transform != burfParentTransform.transform)
            {
                Destroy(existingItem.gameObject);
            }
        }
    }

    public void Disable()
    {
        DisableBurfPopup();
        burfUi.SetActiveState(false);
    }
}
