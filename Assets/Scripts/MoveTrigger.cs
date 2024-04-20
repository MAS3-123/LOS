using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public bool moveOn = false;

    public void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player") && _collision.gameObject.tag == "Player")
        {
            moveOn = true;
        }
    }
}
