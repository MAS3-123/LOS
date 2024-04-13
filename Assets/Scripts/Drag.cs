using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{

    // [ColorUsage(true, true)] public Color color; Color > HDR 컬러 설정하는 법

    //[SerializeField] bool Test = false;
    //[SerializeField] GameObject objTest;
    [SerializeField] RectTransform barRect;
    [SerializeField] Canvas canvas;

    private Vector2 barPos;
    private Vector2 mousePos;
    private Vector2 distancePos;
    private Vector2 barLimmitPos;

    private Vector3 canvasScale;
    private Vector3 barScale;

    private bool barPointerCheck = false;

    float sWidth = 0f;
    float sHeight = 0f;
    float barWidth = 0f;
    float barHeight = 0f;

    RectTransform rect;

    Vector2 limitPosX;
    Vector2 limitPosY;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        barWidth = rect.rect.width;
        barHeight = rect.rect.height;

        sWidth = Screen.width;
        sHeight = Screen.height;

        canvasScale = canvas.gameObject.transform.localScale;
        barScale = gameObject.transform.localScale;

        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
        barHeight = barRect.rect.height;

        limitPosX = new Vector2(barWidth * (barScale.x * 0.5f) * canvasScale.x, sWidth - barWidth * (barScale.x * 0.5f) * canvasScale.x); // x축 최대 최소 저장 벡터값
        limitPosY = new Vector2(barHeight * (barScale.y) * canvasScale.y, sHeight - barHeight * (barScale.y) * canvasScale.y); // y축 최대 최소 저장 벡터값
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if(clickedObject.tag == "Bar")
        {
            barPointerCheck = true;
        }

        barPos = gameObject.transform.position;
        mousePos = eventData.position;

        distancePos = mousePos - barPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = new Vector3(
            Mathf.Clamp(eventData.position.x - distancePos.x, limitPosX.x, limitPosX.y) , 
            Mathf.Clamp(eventData.position.y - distancePos.y, limitPosY.x, limitPosY.y) ,
            gameObject.transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(barPointerCheck == false)
        {
            return;
        }
        barPointerCheck = false;
    }
}
