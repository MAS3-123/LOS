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

    public void OnDrop(PointerEventData eventData) // 아이템의 OnEndDrag 보다 먼저 호출 됨 / 아이템을 슬롯에 놨을 때 호출
    {
        if (eventData.pointerDrag.gameObject != null) // 아이템이 없지 않으면
        {
            UIItem tem = eventData.pointerDrag.gameObject.GetComponent<UIItem>();
            Transform temTrs = tem.trsBeforeParent; // 드래그한 오브젝트의 이전 부모트랜스폼.
            RectTransform dragRect = eventData.pointerDrag.GetComponent<RectTransform>(); // dragRect 는 움직이는 오브젝트의 RectTransform을 참조한다.
            eQWERPSlot beforeSlot = temTrs.GetComponent<UISlot>().slot; // 드래그한 오브젝트의 qwer슬롯

            if(beforeSlot != eQWERPSlot.None && slot == eQWERPSlot.None) // QWERP 슬롯에서 인벤토리 Slot으로 옮겼을경우 
            {
                Debug.Log("스킬 슬롯에서 인벤토리 슬롯으로 아이템을 옮길 수 없습니다.");
                skip = true;
            }
            else if(beforeSlot != eQWERPSlot.None && slot != eQWERPSlot.None) // QWERP 슬롯간 이동이 있었을 때
            {
                shutoff = ShutOffMove(temTrs); //QWER<> P 슬롯간 이동을 하려 했을 경우
                if (shutoff)
                {
                    skip = true;
                }
            }

            if(skip != true)
            {
                eventData.pointerDrag.transform.SetParent(transform); // 내가 드래그한 오브젝트의 부모를 끌어다 놓은 슬롯으로 한다.(내가 끌어다 놓은 슬롯의 자식으로 드래그한 오브젝트를 넣는다.)
                dragRect.position = rect.position; // dragRect.position 은 UIslot(놓으려는 슬롯)의 rect 포지션을 참조한다.

                if (transform.childCount > 1) // 오브젝트가 있는 슬롯에 올려 놨을 때
                {
                    GameObject obj = transform.GetChild(0).gameObject; // 그 슬롯의 오브젝트
                    UIItem beforeItem = obj.GetComponent<UIItem>(); // 그 슬롯의 오브젝트 uiitem sc
                    obj.transform.SetParent(temTrs);
                    obj.transform.position = temTrs.position;

                    if (beforeSlot != eQWERPSlot.None)
                    {
                        MoveItem(beforeSlot, beforeItem);
                    }
                }

                SkillSlot(slot, tem); // actvie / passive 슬롯에 오브젝트 옮길 때
            }
            else 
            {
                Debug.Log("오브젝트 제자리로");
                GameObject obj = eventData.pointerDrag.gameObject;
                obj.transform.SetParent(temTrs);
                obj.transform.position = temTrs.position;

                if(temTrs.GetComponent<UISlot_A>() != null) // 이전 슬롯이 Active 슬롯일 때
                {
                    ActiveSkillSlot aSlot = temTrs.GetComponentInParent<ActiveSkillSlot>();
                    aSlot.SetUiItem(beforeSlot, tem);
                }
                else if(temTrs.GetComponent<UISlot_P>() != null) // 이전 슬롯이 Passive 슬롯일 때
                {
                    PassiveSkillSlot pSlot = temTrs.GetComponentInParent<PassiveSkillSlot>();
                    pSlot.SetUiItem(beforeSlot, tem);
                }
                else // QWER < > P간 오브젝트 이동일 때
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

    public virtual void SkillSlot(eQWERPSlot _slot, UIItem _item) // Slot > QWERP슬롯으로 아이템 옮길 때 쓸 함수
    {

    }
    public virtual void MoveItem(eQWERPSlot _beforeSlot, UIItem _beforeItem) // Active slot 간 아이템 옮길 때 쓸 함수
    {

    }
    public virtual bool ShutOffMove(Transform _temTrs) // QWER <  > P 서로 오브젝트 이동 차단 트리거
    {
        return false;
    }
    public virtual void ReturnObj(Transform _temTrs, UIItem _item) // 트리거 됐을 때 오브젝트 제자리로 가는 함수
    {

    }
}