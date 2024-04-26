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
        image = GetComponent<Image>(); // image 는 이 스크립트를 갖고 있는 오브젝트의 Image 컴포넌트를 참조한다.
        rect = GetComponent<RectTransform>(); // rect 함수는 이 스크립트를 갖고 있는 오브젝트의 rectTrasform이다.
        defaultColor = image.color; // defaultColor는 이 오브젝트의 color이다.
    }

    public void OnDrop(PointerEventData eventData) // 아이템의 OnEndDrag 보다 먼저 호출 됨 / 아이템을 슬롯에 놨을 때 호출
    {
        if (eventData.pointerDrag.gameObject != null) // 아이템이 없지 않으면
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // 드래그한 오브젝트의 이전 부모트랜스폼.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect 는 움직이는 오브젝트의 RectTransform을 참조한다.
            eQWERPSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // 드래그한 오브젝트의 qwer슬롯

            if(beforeSlot != eQWERPSlot.None && slot == eQWERPSlot.None) // QWERP 슬롯에서 인벤토리 Slot으로 옮겼을경우 return
            {
                Debug.Log("스킬 슬롯에서 인벤토리 슬롯으로 아이템을 옮길 수 없습니다.");
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

            eventData.pointerDrag.transform.SetParent(transform); // 내가 드래그한 오브젝트의 부모를 끌어다 놓은 슬롯으로 한다.(내가 끌어다 놓은 슬롯의 자식으로 드래그한 오브젝트를 넣는다.)
            dragRect.position = rect.position; // dragRect.position 은 UIslot(놓으려는 슬롯)의 rect 포지션을 참조한다.

            if (transform.childCount > 1) // 오브젝트가 있는 슬롯에 올려 놨을 때
            {
                GameObject obj = transform.GetChild(0).gameObject; // 그 슬롯의 오브젝트
                UIItem beforeItem = obj.GetComponent<UIItem>(); // 그 슬롯의 오브젝트 uiitem sc

                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                MoveItem(beforeSlot, beforeItem); // P 슬롯은 하나라 A슬롯만
            }

            SkillSlot(slot, tem); // actvie / passive 슬롯에 오브젝트 옮길 때
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

    public virtual void SkillSlot(eQWERPSlot _slot, UIItem _item) // Slot > QWERP슬롯으로 아이템 옮길 때 쓸 함수
    {

    }
    public virtual void MoveItem(eQWERPSlot _beforeSlot, UIItem _beforeItem) // Active slot 간 아이템 옮길 때 쓸 함수
    {

    }
    public virtual bool ShutOffMove(Transform _temTrs) // QWER <  > P 서로 오브젝트 이동 차단 함수
    {
        return false;
    }
}