using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{

    // [ColorUsage(true, true)] public Color color; Color > HDR 컬러 설정하는 법

    [SerializeField] bool Test = false;
    [SerializeField] GameObject objTest;
    [SerializeField] RectTransform barRect;

    private Vector2 barPos;
    private Vector2 mousePos;
    private Vector2 distancePos;

    private bool onDragCheck = false;
    private bool barPointerCheck = false;
    private bool mouseCheck = false;
    private bool outPos = true;

    float sWidth = 0f;
    float sHeight = 0f;
    float barWidth = 0f;
    float barHeight = 0f;
    float fmous = 0f;
    float xPos = 0f;
    float yPos = 0f;


    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if(clickedObject.tag == "Bar")
        {
            barPointerCheck = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        float ratioX = gameObject.transform.position.x / sWidth;
        float ratioY = gameObject.transform.position.y / sHeight;

        //Debug.Log($"x = {ratioX} , y = {ratioY}");
        //Debug.Log($"posx = {gameObject.transform.position.x} , posy = {gameObject.transform.position.y}");


        if (barPointerCheck == false)
        {
            return;
        }

        barPos = gameObject.transform.position;
        mousePos = eventData.position;

        if(onDragCheck == false)
        {
            DragPos();
        }
        //gameObject.transform.position = eventData.position - distancePos;
        
        //min = 0 max = 화면의 끝
        

        #region 나중에 한번 보기
        if (ratioX < 0.1f)
        {
            LimitBarPos(80.5f, eventData.position, "X");
        }
        else if (ratioX > 0.91)
        {
            LimitBarPos(745f, eventData.position, "X");
        }
        if (ratioY < 0.358)
        {
            LimitBarPos(224f, eventData.position, "Y");
        }
        else if (ratioY > 0.985)
        {
            LimitBarPos(617f, eventData.position, "Y");
        }
        if (outPos)
        {
            gameObject.transform.position = eventData.position - distancePos;
            mouseCheck = false;
        }
        #endregion
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(barPointerCheck == false)
        {
            return;
        }

        gameObject.transform.position = eventData.position - distancePos;

        onDragCheck = false;
        barPointerCheck = false;
    }

    private void Start()
    {
        sWidth = Screen.width;
        sHeight = Screen.height;


        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
        barHeight = barRect.rect.height;
    }


    private void DragPos()
    {
        distancePos = mousePos - barPos;
        onDragCheck = true;
    }

    private void LimitBarPos(float _limitPos, Vector2 _vec, string _XorY)
    {
        outPos = false;

        switch (_XorY)
        {
            case "X":
                if (mouseCheck == false)
                {
                    mouseCheck = true;
                    fmous = _vec.x;
                    xPos = _limitPos;
                }
                if (_limitPos < 400)
                {
                    if (mousePos.x > fmous)
                    {
                        xPos = _vec.x - distancePos.x;
                        outPos = true;
                    }
                }
                else if (_limitPos > 400)
                {
                    if (mousePos.x < fmous)
                    {
                        xPos = _vec.x - distancePos.x;
                        outPos = true;
                    }
                }
                yPos = _vec.y - distancePos.y;
                gameObject.transform.position = new Vector2(xPos, yPos);

                break;

            case "Y":
                if (mouseCheck == false)
                {
                    mouseCheck = true;
                    fmous = _vec.y;
                    yPos = _limitPos;
                }
                if (_limitPos < 400)
                {
                    if (mousePos.y > fmous)
                    {
                        yPos = _vec.y - distancePos.y;
                        outPos = true;
                    }
                }
                else if (_limitPos > 400)
                {
                    if (mousePos.y < fmous)
                    {
                        yPos = _vec.y - distancePos.y;
                        outPos = true;
                    }
                }
                xPos = _vec.x - distancePos.x;
                gameObject.transform.position = new Vector2(xPos, yPos);

                break;
        }

    }
}
