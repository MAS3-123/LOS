using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform trsCanvas;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Image image;

    public Transform trsBeforeParent;

    void Start()
    {
        trsCanvas = FindObjectOfType<Canvas>().transform; // trsCavas는 Hiarachy에 있는 최상위 오브젝트부터 찾기 시작하고 가장 첫번째 오브젝트 중에 Canvas를 가진 오브젝트의 transform이다.
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) // 드래그 시작 했을 때
    {
        trsBeforeParent = transform.parent ;// 이전으로 돌아갈 transform 값을 이 오브젝트의 부모 transform으로 설정.

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
        Debug.Log($" 잡았던 오브젝트 = {LayerMask.LayerToName(gameObject.layer)} ");
        Debug.Log($" 놓으려 했던 슬롯 = {LayerMask.LayerToName(trs.gameObject.layer)} ");

        if(transform.parent == trsCanvas || LayerMask.LayerToName(gameObject.layer) != LayerMask.LayerToName(trs.gameObject.layer)) // 오브젝트를 놓은 후 제자리로 돌아가는 부분
        {
            transform.SetParent(trsBeforeParent); // OnBeginDrag 에서 설정했던 슬롯의 transform으로 되돌아감.
            rect.position = trsBeforeParent.GetComponentInParent<RectTransform>().position; // 슬롯의 중앙에 위치해야 하니 그 슬롯의 rectTrasform 을 가져와서 사용.
            Debug.Log("슬롯 밖에 두었거나 맞는 스킬 슬롯에 넣어주세요");
        }
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
