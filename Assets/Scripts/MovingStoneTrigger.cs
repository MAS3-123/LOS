using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStoneTrigger : MonoBehaviour
{
    public float routineF = 4f;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _collision.gameObject.tag == "Move Object")
        {
            routineF = -routineF;
        }
    }
}
