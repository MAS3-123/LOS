using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private Vector2 barPos;
    private Vector2 mousePos;
    private Vector2 distancePos;

    private bool onDragCheck = false;
    private bool barPointerCheck = false;

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
        if(barPointerCheck == false)
        {
            return;
        }

        barPos = gameObject.transform.position;
        mousePos = eventData.position;

        if(onDragCheck == false)
        {
            DragPos();
        }

        gameObject.transform.position = eventData.position - distancePos;
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

    private void DragPos()
    {
        distancePos = mousePos - barPos;
        onDragCheck = true;
    }
}
