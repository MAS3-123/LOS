using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovingStone_Vertical : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigid;

    private bool moveOn = false;
    private bool startSpeed = false;

    private float moveY = 0f;
    private float routineF = 0f;
    private float time = 0f;

    public float p_routineF
    {
        get { return routineF; }
        set { routineF = value; }
    }

    private void Update()
    {
        MovingOn();
    }

    private void MovingOn()
    {
        if (moveOn == true)
        {
            if(startSpeed == false)
            {
                p_routineF = 4;
                startSpeed = true;
            }
            time = 0;
        }
        else
        {
            if (startSpeed == true)
            {
                if (time > 2f)
                {
                    p_routineF = -4f;
                    startSpeed = false;
                }
            }
            time += Time.deltaTime;
        }
        moveY = p_routineF;
        myRigid.velocity = new Vector2(myRigid.velocity.x, moveY);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            moveOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            moveOn = false;
        }
    }
}
