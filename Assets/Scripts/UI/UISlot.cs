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
        image = GetComponent<Image>(); // image 는 이 스크립트를 갖고 있는 오브젝트의 Image 컴포넌트를 참조한다.
        rect = GetComponent<RectTransform>(); // rect 함수는 이 스크립트를 갖고 있는 오브젝트의 rectTrasform이다.
        defaultColor = image.color; // defaultColor는 이 오브젝트의 color이다.

        if (slot != eQWERSlot.None && pSlot == ePSlot.None)
        {
            activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
        }
        else if(slot == eQWERSlot.None && pSlot != ePSlot.None)
        {
            passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
        }
    }

    public void OnDrop(PointerEventData eventData) // 아이템의 OnEndDrag 보다 먼저 호출 됨 / 아이템을 슬롯에 놨을 때 호출
    {
        if (eventData.pointerDrag.gameObject != null) // 아이템이 없지 않으면
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // 드래그한 오브젝트의 이전 부모트랜스폼.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect 는 움직이는 오브젝트의 RectTransform을 참조한다.

            eventData.pointerDrag.transform.SetParent(transform); // 내가 드래그한 오브젝트의 부모를 끌어다 놓은 슬롯으로 한다.(내가 끌어다 놓은 슬롯의 자식으로 드래그한 오브젝트를 넣는다.)
            dragRect.position = rect.position; // dragRect.position 은 UIslot(놓으려는 슬롯)의 rect 포지션을 참조한다.

            if (transform.childCount > 1) // 오브젝트가 있는 슬롯에 올려 놨을 때
            {
                GameObject obj = transform.GetChild(0).gameObject; // 그 슬롯의 오브젝트
                UIItem beforeItem = obj.GetComponent<UIItem>(); // 그 슬롯의 오브젝트 uiitem sc
                eQWERSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // 드래그한 오브젝트의 qwer슬롯

                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                if (beforeSlot != eQWERSlot.None) // 드래그한 오브젝트의 이전 슬롯이 qwer일경우
                {
                    if(slot == eQWERSlot.None) // 드래그한 오브젝트의 현재 슬롯이 인벤토리 슬롯일경우
                    {
                        activeSkillSlot = temTrs.transform.GetComponentInParent<ActiveSkillSlot>();
                    }
                    activeSkillSlot.SetUiItem(beforeSlot, beforeItem);
                }
            }

            if(slot != eQWERSlot.None)
            {
                UIItem item = eventData.pointerDrag.gameObject.GetComponent<UIItem>(); // item 은 내가 드래그한 오브젝트의 UIItem 컴포넌트를 참조한다.
                activeSkillSlot.SetUiItem(slot, item); // activeSkillSlot의 SetUiItem 함수를 사용함.
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