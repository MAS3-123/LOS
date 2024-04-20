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
        Debug.Log($" ��Ҵ� ������Ʈ = {LayerMask.LayerToName(gameObject.layer)} ");
        Debug.Log($" ������ �ߴ� ���� = {LayerMask.LayerToName(trs.gameObject.layer)} ");

        if(transform.parent == trsCanvas || LayerMask.LayerToName(gameObject.layer) != LayerMask.LayerToName(trs.gameObject.layer))
        {
            transform.SetParent(trsBeforeParent);
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position;
            Debug.Log("���� �ۿ� �ξ��ų� �´� ��ų ���Կ� �־��ּ���");
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(SpriteRenderer _spr)
    {
        if (image == null)
        {
            image = GetComponent<SpriteRenderer>();
            //image.SetNativeSize(); //������ ������ ������� ����
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
