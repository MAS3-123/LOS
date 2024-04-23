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
        image = GetComponent<Image>(); // image 는 이 스크립트를 갖고 있는 오브젝트의 Image 컴포넌트를 참조한다.
        rect = GetComponent<RectTransform>(); // rect 함수는 이 스크립트를 갖고 있는 오브젝트의 rectTrasform이다.
        defaultColor = image.color; // defaultColor는 이 오브젝트의 color이다.

        if (slot != eQWERSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
    }

    public void OnDrop(PointerEventData eventData) // 아이템의 OnEndDrag 보다 먼저 호출 됨 / 아이템을 슬롯에 놨을 때 호출
    {
        if (eventData.pointerDrag.gameObject != null) // 아이템이 없지 않으면
        {

            if (slot != eQWERSlot.None) // QWER슬롯이라면(active skill slot에 놓았다면)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>(); // item 은 내가 드래그한 오브젝트의 UIItem 컴포넌트를 참조한다.
                activeSkillSlot.SetUiItem(slot, item); // activeSkillSlot의 SetUiItem 함수를 사용함.
            }

            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent;

            eventData.pointerDrag.transform.SetParent(transform); // 내가 드래그한 오브젝트의 부모를 끌어다 놓은 슬롯으로 한다.(내가 끌어다 놓은 슬롯의 자식으로 드래그한 오브젝트를 넣는다.)
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect 는 움직이는 오브젝트의 RectTransform을 참조한다.
            dragRect.position = rect.position; // dragRect.position 은 UIslot(즉 놓으려는 슬롯)의 rect 포지션을 참조한다.

            Debug.Log($"{gameObject.name} 슬롯에 둠");
            Debug.Log($"{eventData.pointerDrag} 집은 아이템");

            if (transform.childCount > 1)
            {
                Debug.Log($"슬롯 과부하");
                GameObject obj = transform.GetChild(0).gameObject;
                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;
                //if (slot != eQWERSlot.None) // QWER슬롯이라면(active skill slot에 놓았다면)
                //{
                //    Debug.Log(obj.transform.parent.name);// = 이전거 뜸
                //    UIItem beforeSlot = obj.gameObject.GetComponent<UIItem>(); // item 은 내가 드래그한 오브젝트의 UIItem 컴포넌트를 참조한다.
                //    activeSkillSlot.SetUiItem(slot, beforeSlot); // activeSkillSlot의 SetUiItem 함수를 사용함.
                //}
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white; // 커서를 올리면 이미지를 하얀색으로 한다.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor; // 커서를 빼면 이미지를 기본 색으로 바꾼다.
    }


}
