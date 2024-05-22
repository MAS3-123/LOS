using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStone_Vertical : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigid;
    [SerializeField] private MoveTrigger moveTri;
    [SerializeField] private MovingStoneTrigger movingTri;

    private bool moveOn = false;

    private float moveY = 0f;
    private float routineF = 0f;

    private void Start()
    {

    }

    private void Update()
    {
        MovingOn();
    }

    private void MovingOn()
    {
        moveOn = moveTri.moveOn;
        routineF = movingTri.routineF;

        if (moveOn == true)
        {
            moveY = routineF;
        }
        else
        {

        }

        myRigid.velocity = new Vector2(myRigid.velocity.x, moveY);
    }
}
