using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public abstract class PointEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<GameObject> OnPointerEntered = delegate { };
    public event Action<GameObject> OnPointerExited = delegate { };

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEntered(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExited(this.gameObject);
    }
}
