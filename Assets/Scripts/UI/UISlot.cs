using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using System;

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


public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERPSlot slot;
    public eSlotType slotType;

    private Image image;
    private RectTransform rect;
    private Color defaultColor;
    public GameObject TMIobj;
    private bool shutoff = false;
    private bool skip = false;

    void Start()
    {
        image = GetComponent<Image>(); 
        rect = GetComponent<RectTransform>(); 
        defaultColor = image.color; 
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // �巡���� ������Ʈ�� ���� �θ�Ʈ������.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.
            eQWERPSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // �巡���� ������Ʈ�� qwer����

            if(beforeSlot != eQWERPSlot.None && slot == eQWERPSlot.None) // QWERP ���Կ��� �κ��丮 Slot���� �Ű������ 
            {
                Debug.Log("��ų ���Կ��� �κ��丮 �������� �������� �ű� �� �����ϴ�.");
                skip = true;
            }
            else if(beforeSlot != eQWERPSlot.None && slot != eQWERPSlot.None) // QWERP ���԰� �̵��� �־��� ��
            {
                shutoff = ShutOffMove(temTrs); //QWER<> P ���԰� �̵��� �Ϸ� ���� ���
                if (shutoff)
                {
                    skip = true;
                }
            }

            if(skip != true)
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

                if(temTrs.GetComponent<UISlot_A>() != null) // ���� ������ Active ������ ��
                {
                    ActiveSkillSlot aSlot = temTrs.GetComponentInParent<ActiveSkillSlot>();
                    aSlot.SetUiItem(beforeSlot, tem);
                }
                else if(temTrs.GetComponent<UISlot_P>() != null) // ���� ������ Passive ������ ��
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
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white;
        if(gameObject.transform.childCount > 0 )
        {
            Transform skillObj = gameObject.transform.GetChild(0);
            GameObject obj = InventoryManager.Instance.TMI_Object;
            TMI tmi = obj.GetComponent<TMI>();

            tmi.image.sprite = skillObj.GetComponent<Image>().sprite;
            tmi.slimName.text = skillObj.name.Substring(0, skillObj.name.Length - 6);

            if (eventData.pointerEnter.gameObject != null )
            {
                GameObject objPointEnter = eventData.pointerEnter.gameObject;
                UIItem sc = objPointEnter.GetComponent<UIItem>();
                if (sc != null)
                {
                    tmi.tmi.text = sc.tmi;
                }
            }
            TMIobj = Instantiate(obj, gameObject.transform.position + new Vector3(-110f, 0f, 0f), Quaternion.identity, GameObject.Find("UI Canvas").gameObject.transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
        Destroy(TMIobj);
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