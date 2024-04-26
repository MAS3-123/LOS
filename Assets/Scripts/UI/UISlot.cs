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

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERPSlot slot;

    private Image image;
    private RectTransform rect;
    private Color defaultColor;
    public List<Transform> trsSlot = new List<Transform>();
    private bool shutoff = false;

    void Start()
    {
        image = GetComponent<Image>(); // image �� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� Image ������Ʈ�� �����Ѵ�.
        rect = GetComponent<RectTransform>(); // rect �Լ��� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� rectTrasform�̴�.
        defaultColor = image.color; // defaultColor�� �� ������Ʈ�� color�̴�.
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // �巡���� ������Ʈ�� ���� �θ�Ʈ������.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.
            eQWERPSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // �巡���� ������Ʈ�� qwer����

            if(beforeSlot != eQWERPSlot.None && slot == eQWERPSlot.None) // QWERP ���Կ��� �κ��丮 Slot���� �Ű������ return
            {
                Debug.Log("��ų ���Կ��� �κ��丮 �������� �������� �ű� �� �����ϴ�.");
                return;
            }
            else if(beforeSlot != eQWERPSlot.None && slot != eQWERPSlot.None)
            {
                shutoff = ShutOffMove(temTrs);
                if (shutoff)
                {
                    return;
                }
            }

            eventData.pointerDrag.transform.SetParent(transform); // ���� �巡���� ������Ʈ�� �θ� ����� ���� �������� �Ѵ�.(���� ����� ���� ������ �ڽ����� �巡���� ������Ʈ�� �ִ´�.)
            dragRect.position = rect.position; // dragRect.position �� UIslot(�������� ����)�� rect �������� �����Ѵ�.

            if (transform.childCount > 1) // ������Ʈ�� �ִ� ���Կ� �÷� ���� ��
            {
                GameObject obj = transform.GetChild(0).gameObject; // �� ������ ������Ʈ
                UIItem beforeItem = obj.GetComponent<UIItem>(); // �� ������ ������Ʈ uiitem sc

                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                MoveItem(beforeSlot, beforeItem); // P ������ �ϳ��� A���Ը�
            }

            SkillSlot(slot, tem); // actvie / passive ���Կ� ������Ʈ �ű� ��
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white; // Ŀ���� �ø��� �̹����� �Ͼ������ �Ѵ�.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor; // Ŀ���� ���� �̹����� �⺻ ������ �ٲ۴�.
    }

    public virtual void SkillSlot(eQWERPSlot _slot, UIItem _item) // Slot > QWERP�������� ������ �ű� �� �� �Լ�
    {

    }
    public virtual void MoveItem(eQWERPSlot _beforeSlot, UIItem _beforeItem) // Active slot �� ������ �ű� �� �� �Լ�
    {

    }
    public virtual bool ShutOffMove(Transform _temTrs) // QWER <  > P ���� ������Ʈ �̵� ���� �Լ�
    {
        return false;
    }
}