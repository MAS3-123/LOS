using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJump : MonoBehaviour
{
    private Player player;
    private Rigidbody2D playerRigid;

    private Vector2 moveDir;

    private float time = 0;

    private int speed = 10;

    IEnumerator Fly()
    {
        player.p_superJumpOn = true;
        StartCoroutine(PlayerPlaying());
        while (true)
        {
            yield return new WaitForSeconds(1f);
            time += 1f;
            if (time >= 10f)
            {
                player.p_superJumpOn = false;
                break;
            }
        }
    }

    IEnumerator PlayerPlaying()
    {
        playerRigid = player.p_myRigid;
        while (true)
        {
            yield return null;
            if (player.p_superJumpOn)
            {
                moveDir.x = Input.GetAxis("Horizontal");
                moveDir.y = Input.GetAxis("Vertical");
                playerRigid.velocity = new Vector2(moveDir.x, moveDir.y) * speed;
            }
            else
            { 
                yield return new WaitForSeconds(1);
                break;
            }
        }
    }

    private void Start()
    {
        player = gameObject.transform.parent.GetComponent<Player>();
        StartCoroutine(Fly());
    }
}
