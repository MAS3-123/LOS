using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragBarPosition : MonoBehaviour
{
    [SerializeField] bool Test = false;
    [SerializeField] GameObject objTest;
    [SerializeField] RectTransform barRect;

    Vector3 pos;

    float sWidth = 0f;
    float sHeight = 0f;
    float barWidth = 0f;
    float barHeight = 0f;


    void Start()
    {
        barRect = GetComponent<RectTransform>();
        barWidth = barRect.rect.width;
        barHeight = barRect.rect.height;

        sWidth = Screen.width;
        sHeight = Screen.height;
        string value = $"sWidth = {sWidth}, sHeight = {sHeight}";
        Debug.Log(value);
    }

    void Update()
    {

        float ratioX = pos.x / sWidth;
        float ratioY = pos.y / sHeight;


        if (Test)
        {
            Debug.Log($"x = {ratioX} , y = {ratioY}");
            Debug.Log($"posx = {pos.x} , posy = {pos.y}");
        }


    }
}
