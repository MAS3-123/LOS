using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStoneTrigger : MonoBehaviour
{
    public float routineF = 4f;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _collision.gameObject.tag == "Move Object_Horizontal")
        {
            MovingStone_Horizontal sc = _collision.gameObject.GetComponent<MovingStone_Horizontal>();
            routineF = -routineF;
            sc.p_routineF = routineF;
        }
        else if(_collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _collision.gameObject.tag == "Move Object_Vertical")
        {
            MovingStone_Vertical sc = _collision.gameObject.GetComponent<MovingStone_Vertical>();
            sc.p_routineF = 0f;
        }
    }
}
