using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{

    [SerializeField] CircleCollider2D colli;

    public bool ground = false;


    void Update()
    {
        transform.position += new Vector3(1.0f, 0f, 0f) * Time.deltaTime;
    }
}
