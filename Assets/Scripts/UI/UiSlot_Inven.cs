using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum eQWERPSlot
{
    None,
    Q,
    W,
    E,
    R,
    P
}
public enum eSlotType
{
    Inven,
    Active,
    Passive
}
public class UiSlot_Inven : UISlot, IDropHandler
{
    public eQWERPSlot slot;
    public eSlotType slotType;

    private bool skip;
    private bool shutoff;
    public override void Start()
    {
        base.Start();
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // �巡���� ������Ʈ�� ���� �θ�Ʈ������.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.
            eQWERPSlot beforeSlot = temTrs.GetComponent<UiSlot_Inven>().slot; // �巡���� ������Ʈ�� qwer����

            if (beforeSlot != eQWERPSlot.None && slot == eQWERPSlot.None) // QWERP ���Կ��� �κ��丮 Slot���� �Ű������ 
            {
                Debug.Log("��ų ���Կ��� �κ��丮 �������� �������� �ű� �� �����ϴ�.");
                skip = true;
            }
            else if (beforeSlot != eQWERPSlot.None && slot != eQWERPSlot.None) // QWERP ���԰� �̵��� �־��� ��
            {
                shutoff = ShutOffMove(temTrs); //QWER<> P ���԰� �̵��� �Ϸ� ���� ���
                if (shutoff)
                {
                    skip = true;
                }
            }

            if (skip != true)
            {
                eventData.pointerDrag.transform.SetParent(transform); // ���� �巡���� ������Ʈ�� �θ� ����� ���� �������� �Ѵ�.(���� ����� ���� ������ �ڽ����� �巡���� ������Ʈ�� �ִ´�.)
                dragRect.position = rect.position; // dragRect.position �� UIslot(�������� ����)�� rect �������� �����Ѵ�.

                if (transform.childCount > 1) // ������Ʈ�� �ִ� ���Կ� �÷� ���� ��
                {
                    GameObject obj = transform.GetChild(0).gameObject; // �� ������ ������Ʈ
                    UIItem beforeItem = obj.GetComponent<UIItem>(); // �� ������ ������Ʈ uiitem sc
                    obj.transform.SetParent(temTrs);
                    obj.transform.position = temTrs.position;

                    if (beforeSlot != eQWERPSlot.None)
                    {
                        MoveItem(beforeSlot, beforeItem);
                    }
                }

                SkillSlot(slot, tem); // actvie / passive ���Կ� ������Ʈ �ű� ��
            }
            else
            {
                Debug.Log("������Ʈ ���ڸ���");
                GameObject obj = eventData.pointerDrag.gameObject;
                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                if (temTrs.GetComponent<UISlot_A>() != null) // ���� ������ Active ������ ��
                {
                    ActiveSkillSlot aSlot = temTrs.GetComponentInParent<ActiveSkillSlot>();
                    aSlot.SetUiItem(beforeSlot, tem);
                }
                else if (temTrs.GetComponent<UISlot_P>() != null) // ���� ������ Passive ������ ��
                {
                    PassiveSkillSlot pSlot = temTrs.GetComponentInParent<PassiveSkillSlot>();
                    pSlot.SetUiItem(beforeSlot, tem);
                }
                else // QWER < > P�� ������Ʈ �̵��� ��
                {
                    ReturnObj(temTrs, tem);
                }
                skip = false;
            }
            if (TMIobj != null)
            {
                Destroy(TMIobj);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public virtual void SkillSlot(eQWERPSlot _slot, UIItem _item) // Slot > QWERP�������� ������ �ű� �� �� �Լ�
    {

    }
    public virtual void MoveItem(eQWERPSlot _beforeSlot, UIItem _beforeItem) // Active slot �� ������ �ű� �� �� �Լ�
    {

    }
    public virtual bool ShutOffMove(Transform _temTrs) // QWER <  > P ���� ������Ʈ �̵� ���� Ʈ����
    {
        return false;
    }
    public virtual void ReturnObj(Transform _temTrs, UIItem _item) // Ʈ���� ���� �� ������Ʈ ���ڸ��� ���� �Լ�
    {

    }
}