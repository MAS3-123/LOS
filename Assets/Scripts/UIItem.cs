using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Transform trsCanvas;
    private Transform trsBeforeParent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image image;

    public void OnBeginDrag(PointerEventData eventData)
    {
        trsBeforeParent = transform.parent;

        transform.SetParent(trsCanvas);

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == trsCanvas)
        {
            transform.SetParent(trsBeforeParent);
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Sprite _spr)
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = _spr;
        image.SetNativeSize(); //원본과 동일한 사이즈로 변경
    }

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

}
