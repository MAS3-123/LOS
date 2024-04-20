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
    private SpriteRenderer image;

    public void OnBeginDrag(PointerEventData eventData)
    {
        trsBeforeParent = transform.parent ;

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
        Transform trs = transform.parent;
        Debug.Log($" 잡았던 오브젝트 = {LayerMask.LayerToName(gameObject.layer)} ");
        Debug.Log($" 놓으려 했던 슬롯 = {LayerMask.LayerToName(trs.gameObject.layer)} ");

        if(transform.parent == trsCanvas || LayerMask.LayerToName(gameObject.layer) != LayerMask.LayerToName(trs.gameObject.layer))
        {
            transform.SetParent(trsBeforeParent);
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position;
            Debug.Log("슬롯 밖에 두었거나 맞는 스킬 슬롯에 넣어주세요");
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(SpriteRenderer _spr)
    {
        if (image == null)
        {
            image = GetComponent<SpriteRenderer>();
            //image.SetNativeSize(); //원본과 동일한 사이즈로 변경
        }
        image = _spr;
    }

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void UseItem(Vector3 _vec)
    {
        
    }
}
