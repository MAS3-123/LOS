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
    public string tmi;

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform; // trsCavas�� Hiarachy�� �ִ� �ֻ��� ������Ʈ���� ã�� �����ϰ� ���� ù��° ������Ʈ �߿� Canvas�� ���� ������Ʈ�� transform�̴�.
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log($" ���� ������ = {gameObject.name} �������� ��ųŸ�� = {itemSkillType}");
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ���� ���� ��
    {
        trsBeforeParent = transform.parent ;// �������� ���ư� transform ���� �� ������Ʈ�� �θ� transform���� ����.
        UiSlot_Inven beforeSlot = trsBeforeParent.GetComponent<UiSlot_Inven>();
        Destroy(beforeSlot.TMIobj);
        if (trsBeforeParent.GetComponent<UISlot_A>() != null)
        {
            ActiveSkillSlot activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
            activeSkillSlot.RemoveUiItem(beforeSlot.slot);
        }
        else if (trsBeforeParent.GetComponent<UISlot_P>() != null)
        {
            PassiveSkillSlot passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
            passiveSkillSlot.RemoveUiItem(beforeSlot.slot);
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
        Transform trs = transform.parent ;

        if (transform.parent == trsCanvas) // ������Ʈ�� �κ��丮�� ��ų ���� �ۿ� �ξ��� ��.
        {
            transform.SetParent(trsBeforeParent); // OnBeginDrag ���� �����ߴ� ������ transform���� �ǵ��ư�.
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // ������ �߾ӿ� ��ġ�ؾ� �ϴ� �� ������ rectTrasform �� �����ͼ� ���.
            if (trsBeforeParent.GetComponent<UiSlot_Inven>().slotType != eSlotType.Inven) // ��ų ���Կ��� �ۿ� �ξ��� ��
            {
                Debug.Log("��ų ���Կ��� ������ �ξ����ϴ�.");
                Transform tsr = InventoryManager.Instance.ReturnItem(gameObject);
                rect.position = tsr.position;
                //�κ��丮 �������� �̵�
                if(trsBeforeParent.GetComponent<UiSlot_Inven>().slotType == eSlotType.Passive)
                {
                    Debug.Log("�нú� ���Կ��� �����ϴ�.");
                }
            }
            else
            {
                Debug.Log("���� �ۿ� �ΰų� �´� ��ų���Կ� ���� �ʾҽ��ϴ�.");
            }
        }
        else if(trs.GetComponent<UiSlot_Inven>().slotType != eSlotType.Inven)
        {
            if(trs.GetComponent<UiSlot_Inven>().slotType.ToString() != gameObject.GetComponent<UIItem>().itemSkillType.ToString())
            {
                transform.SetParent(trsBeforeParent); // OnBeginDrag ���� �����ߴ� ������ transform���� �ǵ��ư�.
                rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // ������ �߾ӿ� ��ġ�ؾ� �ϴ� �� ������ rectTrasform �� �����ͼ� ���.
            }
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

    public virtual void UseSkill(Vector3 _vec)
    {
        
    }
}
