using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using System;

public enum eQWERSlot
{
    None,
    Q,
    W,
    E,
    R
}

public enum ePSlot
{
    None,
    P,
}

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERSlot slot;
    public ePSlot pSlot;

    private Image image;
    private RectTransform rect;
    private Color defaultColor;
    private ActiveSkillSlot activeSkillSlot;
    private PassiveSkillSlot passiveSkillSlot;
    public List<Transform> trsSlot = new List<Transform>();

    void Start()
    {
        image = GetComponent<Image>(); // image �� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� Image ������Ʈ�� �����Ѵ�.
        rect = GetComponent<RectTransform>(); // rect �Լ��� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� rectTrasform�̴�.
        defaultColor = image.color; // defaultColor�� �� ������Ʈ�� color�̴�.

        if (slot != eQWERSlot.None && pSlot == ePSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
        else if(slot == eQWERSlot.None && pSlot != ePSlot.None)
        {
            passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
        }
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // �巡���� ������Ʈ�� ���� �θ�Ʈ������.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.

            eventData.pointerDrag.transform.SetParent(transform); // ���� �巡���� ������Ʈ�� �θ� ����� ���� �������� �Ѵ�.(���� ����� ���� ������ �ڽ����� �巡���� ������Ʈ�� �ִ´�.)
            dragRect.position = rect.position; // dragRect.position �� UIslot(�������� ����)�� rect �������� �����Ѵ�.

            if (transform.childCount > 1) // ������Ʈ�� �ִ� ���Կ� �÷� ���� ��
            {
                GameObject obj = transform.GetChild(0).gameObject; // �� ������ ������Ʈ
                UIItem beforeItem = obj.GetComponent<UIItem>(); // �� ������ ������Ʈ uiitem sc
                eQWERSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // �巡���� ������Ʈ�� qwer����

                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                if (beforeSlot != eQWERSlot.None) // �巡���� ������Ʈ�� ���� ������ qwer�ϰ��
                {
                    if(slot == eQWERSlot.None) // �巡���� ������Ʈ�� ���� ������ �κ��丮 �����ϰ��
                    {
                        activeSkillSlot = temTrs.transform.GetComponentInParent<ActiveSkillSlot>();
                    }
                    activeSkillSlot.SetUiItem(beforeSlot, beforeItem);
                }
            }

            if(slot != eQWERSlot.None)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>(); // item �� ���� �巡���� ������Ʈ�� UIItem ������Ʈ�� �����Ѵ�.
                activeSkillSlot.SetUiItem(slot, item); // activeSkillSlot�� SetUiItem �Լ��� �����.
            }
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
}