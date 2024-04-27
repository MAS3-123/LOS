using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eItemSkillType
{
    None,
    Active,
    Passive,
}

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform trsCanvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image image;
    public Transform trsBeforeParent;
    public eItemSkillType itemSkillType;

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform; // trsCavas�� Hiarachy�� �ִ� �ֻ��� ������Ʈ���� ã�� �����ϰ� ���� ù��° ������Ʈ �߿� Canvas�� ���� ������Ʈ�� transform�̴�.
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log($" ���� ������ = {gameObject.name} �������� ��ųŸ�� = {itemSkillType.ToString()}");
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ���� ���� ��
    {
        trsBeforeParent = transform.parent ;// �������� ���ư� transform ���� �� ������Ʈ�� �θ� transform���� ����.
        UISlot beforeSlot = trsBeforeParent.GetComponent<UISlot>();
        if (beforeSlot.slot != eQWERPSlot.None)
        {
            if(trsBeforeParent.GetComponent<UISlot_A>() != null)
            {
                ActiveSkillSlot activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
                activeSkillSlot.RemoveUiItem(beforeSlot.slot);
            }
            else if (trsBeforeParent.GetComponent<UISlot_P>() != null)
            {
                PassiveSkillSlot passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
                passiveSkillSlot.RemoveUiItem(beforeSlot.slot);
            }
        }
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
        //Debug.Log($" ��Ҵ� ������Ʈ = {LayerMask.LayerToName(gameObject.layer)} ");
        //Debug.Log($" ������ �ߴ� ���� = {LayerMask.LayerToName(trs.gameObject.layer)} ");
        //Debug.Log($" ���� �������� ������ Ÿ�� = {trs.GetComponent<UISlot>().slotType}, ���� �÷����� ������Ʈ�� ��ų Ÿ�� = {gameObject.GetComponent<UIItem>().itemSkillType} ");

        if (transform.parent == trsCanvas) // ������Ʈ�� ���� �� ���ڸ��� ���ư��� �κ� / trs.GetComponent<UISlot>().slotType.ToString() != gameObject.GetComponent<UIItem>().itemSkillType.ToString()
        {
            transform.SetParent(trsBeforeParent); // OnBeginDrag ���� �����ߴ� ������ transform���� �ǵ��ư�.
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // ������ �߾ӿ� ��ġ�ؾ� �ϴ� �� ������ rectTrasform �� �����ͼ� ���.
            Debug.Log("���� �ۿ� �ΰų� �´� ��ų���Կ� ���� �ʾҽ��ϴ�.");
            if (trsBeforeParent.GetComponent<UISlot>().slotType != eSlotType.Inven)
            {
                Debug.Log("��ų ���Կ��� ������ �ξ����ϴ�.");
                Destroy(gameObject);
            }
        }
        //else if(trs.GetComponent<UISlot>().slotType == eSlotType.Inven && trsBeforeParent.GetComponent<UISlot>().slotType != eSlotType.Inven)
        //{
        //    transform.SetParent(trsBeforeParent); // OnBeginDrag ���� �����ߴ� ������ transform���� �ǵ��ư�.
        //    rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // ������ �߾ӿ� ��ġ�ؾ� �ϴ� �� ������ rectTrasform �� �����ͼ� ���.
        //}
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
