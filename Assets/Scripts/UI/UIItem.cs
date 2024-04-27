using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum eItemSkillType
{
    None,
    Active,
    Passive,
}

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform trsCanvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image image;
    public Transform trsBeforeParent;
    public eItemSkillType itemSkillType;

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform; // trsCavas는 Hiarachy에 있는 최상위 오브젝트부터 찾기 시작하고 가장 첫번째 오브젝트 중에 Canvas를 가진 오브젝트의 transform이다.
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log($" 현재 아이템 = {gameObject.name} 아이템의 스킬타입 = {itemSkillType.ToString()}");
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작 했을 때
    {
        trsBeforeParent = transform.parent ;// 이전으로 돌아갈 transform 값을 이 오브젝트의 부모 transform으로 설정.
        UISlot beforeSlot = trsBeforeParent.GetComponent<UISlot>();
        if (beforeSlot.slot != eQWERPSlot.None)
        {
            if(trsBeforeParent.GetComponent<UISlot_A>() != null)
            {
                ActiveSkillSlot activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
                activeSkillSlot.RemoveUiItem(beforeSlot.slot);
            }
            else if (trsBeforeParent.GetComponent<UISlot_P>() != null)
            {
                PassiveSkillSlot passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
                passiveSkillSlot.RemoveUiItem(beforeSlot.slot);
            }
        }
        transform.SetParent(trsCanvas); // 옮기는 순간 부모는 UIcanvs로 바뀜

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // 이 오브젝트의 rect.position 은 eventData.position(드래그중인 포지션 마우스포지션이라 생각하면 편함.)
    }

    public void OnEndDrag(PointerEventData eventData) // 되돌아가는 함수
    {
        Transform trs = transform.parent;
        //Debug.Log($" 잡았던 오브젝트 = {LayerMask.LayerToName(gameObject.layer)} ");
        //Debug.Log($" 놓으려 했던 슬롯 = {LayerMask.LayerToName(trs.gameObject.layer)} ");
        //Debug.Log($" 현재 놓으려는 슬롯의 타입 = {trs.GetComponent<UISlot>().slotType}, 지금 올려놓은 오브젝트의 스킬 타입 = {gameObject.GetComponent<UIItem>().itemSkillType} ");

        if (transform.parent == trsCanvas) // 오브젝트를 놓은 후 제자리로 돌아가는 부분 / trs.GetComponent<UISlot>().slotType.ToString() != gameObject.GetComponent<UIItem>().itemSkillType.ToString()
        {
            transform.SetParent(trsBeforeParent); // OnBeginDrag 에서 설정했던 슬롯의 transform으로 되돌아감.
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // 슬롯의 중앙에 위치해야 하니 그 슬롯의 rectTrasform 을 가져와서 사용.
            Debug.Log("슬롯 밖에 두거나 맞는 스킬슬롯에 두지 않았습니다.");
            if (trsBeforeParent.GetComponent<UISlot>().slotType != eSlotType.Inven)
            {
                Debug.Log("스킬 슬롯에서 밖으로 두었습니다.");
                Destroy(gameObject);
            }
        }
        //else if(trs.GetComponent<UISlot>().slotType == eSlotType.Inven && trsBeforeParent.GetComponent<UISlot>().slotType != eSlotType.Inven)
        //{
        //    transform.SetParent(trsBeforeParent); // OnBeginDrag 에서 설정했던 슬롯의 transform으로 되돌아감.
        //    rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // 슬롯의 중앙에 위치해야 하니 그 슬롯의 rectTrasform 을 가져와서 사용.
        //}
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Sprite _spr)
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            //image.SetNativeSize(); //원본과 동일한 사이즈로 변경
        }
        image.sprite = _spr;
    }

    public virtual void UseItem(Vector3 _vec)
    {
        
    }
}
