using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform trsCanvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image image;

    public Transform trsBeforeParent;

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform; // trsCavas�� Hiarachy�� �ִ� �ֻ��� ������Ʈ���� ã�� �����ϰ� ���� ù��° ������Ʈ �߿� Canvas�� ���� ������Ʈ�� transform�̴�.
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ���� ���� ��
    {
        trsBeforeParent = transform.parent ;// �������� ���ư� transform ���� �� ������Ʈ�� �θ� transform���� ����.

        transform.SetParent(trsCanvas); // �ű�� ���� �θ�� UIcanvs�� �ٲ�

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // �� ������Ʈ�� rect.position �� eventData.position(�巡������ ������ ���콺�������̶� �����ϸ� ����.)
    }

    public void OnEndDrag(PointerEventData eventData) // �ǵ��ư��� �Լ�
    {
        Transform trs = transform.parent;
        Debug.Log($" ��Ҵ� ������Ʈ = {LayerMask.LayerToName(gameObject.layer)} ");
        Debug.Log($" ������ �ߴ� ���� = {LayerMask.LayerToName(trs.gameObject.layer)} ");

        if(transform.parent == trsCanvas || LayerMask.LayerToName(gameObject.layer) != LayerMask.LayerToName(trs.gameObject.layer)) // ������Ʈ�� ���� �� ���ڸ��� ���ư��� �κ�
        {
            transform.SetParent(trsBeforeParent); // OnBeginDrag ���� �����ߴ� ������ transform���� �ǵ��ư�.
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // ������ �߾ӿ� ��ġ�ؾ� �ϴ� �� ������ rectTrasform �� �����ͼ� ���.
            Debug.Log("���� �ۿ� �ξ��ų� �´� ��ų ���Կ� �־��ּ���");
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Sprite _spr)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            //image.SetNativeSize(); //������ ������ ������� ����
        }
        image.sprite = _spr;
    }

    public virtual void UseItem(Vector3 _vec)
    {
        
    }
}
