using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStone_Horizontal : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigid;
    [SerializeField] private MoveTrigger moveTri;
    [SerializeField] private MovingStoneTrigger movingTri;

    private bool moveOn = false;

    private float moveX = 0f;
    private float routineF = 4f;

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
        moveOn = moveTri.moveOn;

        if (moveOn == true)
        {
            moveX = p_routineF;
        }

        myRigid.velocity = new Vector2(moveX, myRigid.velocity.y);
    }
}
