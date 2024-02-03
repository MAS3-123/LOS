using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragBarPosition : MonoBehaviour
{
    Camera cam;

    private Vector2 barPos;
    private Vector2 screenInPos;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = cam.WorldToViewportPoint(gameObject.transform.position);
        //Debug.Log(pos);
    }
}
