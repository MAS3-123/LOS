using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStone : MonoBehaviour
{
    [SerializeField] Rigidbody2D myRigid;
    [SerializeField] MoveTrigger moveTri;
    [SerializeField] MovingStoneTrigger movingTri;

    public bool moveOn = false;

    public float moveX = 0f;
    public float routineF = 0f;

    void Update()
    {
        MovingOn();
    }

    private void MovingOn()
    {
        moveOn = moveTri.moveOn;
        routineF = movingTri.routineF;

        if (moveOn == true)
        {
            moveX = routineF;
        }

        myRigid.velocity = new Vector2(moveX, myRigid.velocity.y);
    }
}
