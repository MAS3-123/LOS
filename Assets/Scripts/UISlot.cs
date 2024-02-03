using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;
    private Color defaultColor;

    public string myLayer = "";

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.active != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>();
            dragRect.position = rect.position;
            Debug.Log($"{gameObject.name} ΩΩ∑‘ø° µ“");
            Debug.Log($"{eventData.pointerDrag} ¡˝¿∫ æ∆¿Ã≈€");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
    }

    void Start()
    {
        image  = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        defaultColor = image.color;
        myLayer = LayerMask.LayerToName(gameObject.layer);
    }

   
}
