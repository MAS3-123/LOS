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

public class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public eQWERSlot slot;

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

        if (slot != eQWERSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
    }

    public void OnDrop(PointerEventData eventData) // �������� OnEndDrag ���� ���� ȣ�� �� / �������� ���Կ� ���� �� ȣ��
    {
        if (eventData.pointerDrag.gameObject != null) // �������� ���� ������
        {

            if (slot != eQWERSlot.None) // QWER�����̶��(active skill slot�� ���Ҵٸ�)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>(); // item �� ���� �巡���� ������Ʈ�� UIItem ������Ʈ�� �����Ѵ�.
                activeSkillSlot.SetUiItem(slot, item); // activeSkillSlot�� SetUiItem �Լ��� �����.
            }

            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent;

            eventData.pointerDrag.transform.SetParent(transform); // ���� �巡���� ������Ʈ�� �θ� ����� ���� �������� �Ѵ�.(���� ����� ���� ������ �ڽ����� �巡���� ������Ʈ�� �ִ´�.)
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect �� �����̴� ������Ʈ�� RectTransform�� �����Ѵ�.
            dragRect.position = rect.position; // dragRect.position �� UIslot(�� �������� ����)�� rect �������� �����Ѵ�.

            Debug.Log($"{gameObject.name} ���Կ� ��");
            Debug.Log($"{eventData.pointerDrag} ���� ������");

            if (transform.childCount > 1)
            {
                Debug.Log($"���� ������");
                GameObject obj = transform.GetChild(0).gameObject;
                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;
                //if (slot != eQWERSlot.None) // QWER�����̶��(active skill slot�� ���Ҵٸ�)
                //{
                //    Debug.Log(obj.transform.parent.name);// = ������ ��
                //    UIItem beforeSlot = obj.gameObject.GetComponent<UIItem>(); // item �� ���� �巡���� ������Ʈ�� UIItem ������Ʈ�� �����Ѵ�.
                //    activeSkillSlot.SetUiItem(slot, beforeSlot); // activeSkillSlot�� SetUiItem �Լ��� �����.
                //}
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
