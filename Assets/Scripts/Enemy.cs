using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject[] included_Skill;

    private bool trigger = false;
    public bool Trigger { set { trigger = value; } }

    private void OnBecameVisible()
    {
        trigger = true;
        Debug.Log($"trigger On objectName ={gameObject.name}");
    }
    private void OnBecameInvisible()
    {
        trigger = false;
        Debug.Log($"trigger Off objectName ={gameObject.name}");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (trigger == false) return;
    }
}
