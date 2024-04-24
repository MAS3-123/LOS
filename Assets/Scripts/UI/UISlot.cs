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
    P,
}

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERPSlot slot;

    private Image image;
    private RectTransform rect;
    private Color defaultColor;
    private ActiveSkillSlot activeSkillSlot;
    public float count = 0f;
    public List<Transform> trsSlot = new List<Transform>();

    void Start()
    {
        image = GetComponent<Image>(); // image �� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� Image ������Ʈ�� �����Ѵ�.
        rect = GetComponent<RectTransform>(); // rect �Լ��� �� ��ũ��Ʈ�� ���� �ִ� ������Ʈ�� rectTrasform�̴�.
        defaultColor = image.color; // defaultColor�� �� ������Ʈ�� color�̴�.

        if (slot != eQWERPSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {
            Transform temTrs = OnDropPos(eventData);
            GameObject obj = transform.GetChild(0).gameObject;
            eQWERPSlot beforeSlot = temTrs.GetComponent<UISlot>().slot;

            if (slot != eQWERPSlot.None) // QWER�����̶��(active skill slot�� ���Ҵٸ�)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>(); // item �� ���� �巡���� ������Ʈ�� UIItem ������Ʈ�� �����Ѵ�.
                activeSkillSlot.SetUiItem(slot, item); // activeSkillSlot�� SetUiItem �Լ��� �����.

                if (transform.childCount > 1 && beforeSlot != eQWERPSlot.None) //qwer ���� > qwer����
                {
                    Debug.Log("qwer ���� > qwer����");
                    UIItem beforeItem = obj.GetComponent<UIItem>();
                    ReturnObject(temTrs, obj);
                    activeSkillSlot.SetUiItem(beforeSlot, beforeItem);
                }
                else if (transform.childCount > 1 && beforeSlot == eQWERPSlot.None)// �κ��丮 ���� > qwer ����
                {
                    Debug.Log("�κ��丮 ���� > qwer����");
                    ReturnObject(temTrs, obj);
                }
            }
            else // �κ��丮 �����̶��
            {
                if (transform.childCount > 1 && beforeSlot == eQWERPSlot.None) // �κ��丮 ���� > �κ��丮 ����
                {
                    Debug.Log("�κ��丮 ���� > �κ��丮 ����");
                    ReturnObject(temTrs, obj);
                }
                else if(transform.childCount > 1 && beforeSlot != eQWERPSlot.None) // qwer ���� > �κ��丮 ����
                {
                    Debug.Log("qwer ���� > �κ��丮 ����");
                    UIItem beforeItem = obj.GetComponent<UIItem>();
                    Debug.Log(beforeItem.gameObject.name);
                    ReturnObject(temTrs, obj);
                    activeSkillSlot.SetUiItem(beforeSlot, beforeItem); // ���� �߻�
                }
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


    private Transform OnDropPos(PointerEventData eventData) // �巡���� ������Ʈ�� ������ �����ִ� �Լ�
    {
        UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
        Transform temTrs = tem.trsBeforeParent; // �巡���� ������Ʈ�� ���� �θ�Ʈ������.

        eventData.pointerDrag.transform.SetParent(transform); // ���� �巡���� ������Ʈ�� �θ� ����� ���� �������� �Ѵ�.(���� ����� ���� ������ �ڽ����� �巡���� ������Ʈ�� �ִ´�.)
        RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.
        dragRect.position = rect.position; // dragRect.position �� UIslot(�� �������� ����)�� rect �������� �����Ѵ�.

        return temTrs;
    }

    private void ReturnObject(Transform _trs, GameObject _obj)
    {
        _obj.transform.SetParent(_trs);
        _obj.transform.position = _trs.position;
    }

}
