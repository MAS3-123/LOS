using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public enum eQWERSlot
{
    None,
    Q,
    W,
    E,
    R
}

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERSlot slot;

    private Image image;
    private RectTransform rect;
    private Color defaultColor;
    private ActiveSkillSlot activeSkillSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject != null)
        {
            if (slot != eQWERSlot.None)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
                activeSkillSlot.SetUiItem(slot, item);
            }

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
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        defaultColor = image.color;

        if (slot != eQWERSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
    }


}
