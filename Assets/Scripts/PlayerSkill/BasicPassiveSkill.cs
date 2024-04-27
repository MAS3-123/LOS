using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPassiveSkill : MonoBehaviour
{
    private bool dubleJump = false;
    public float dubleJumpCount = 0;


    void Update()
    {
        if (Player.Instance.isGround == false && Input.GetKeyDown(KeyCode.Space) == true && dubleJumpCount == 0)
        {
            dubleJump = true;
            dubleJumpCount++;
        }
        DubleJumpe();
    }

    private void DubleJumpe()
    {
        if(Player.Instance.isGround == false)
        {
            if (dubleJump)
            {
                dubleJump = false;
                Player.Instance.verticalVelocity = Player.Instance.jumpForce; // �����̽��ٸ� ������ �� y�� �������� ���� ����.
            }
        }
        else
        {
            dubleJumpCount = 0;
        }
    }
}
