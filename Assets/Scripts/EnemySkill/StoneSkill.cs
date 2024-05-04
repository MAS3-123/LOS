using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneSkill : Enemy
{
    [SerializeField] ParticleSystem ps;

    public bool stemp = false;

    public float curTime = 0;
    private float coolTime = 4f;

    public override void SkillOn()
    {
        if (isJump)
        {
            myRigid.velocity = new Vector2(EPVecX, verticalVelocity);
        }
        else
        {
            myRigid.velocity = new Vector2(0, verticalVelocity);
        }

        if (isGround)
        {
            if (stemp)
            {
                stemp = false;
                if (playerSc.isGround == true && Mathf.Abs(EPVecX) < 2f && Mathf.Abs(EPVecY) < 0.5f) // �Ѵ� ���� ��� �ְ� �Ÿ��� 2���� �۰� ���� ���̰� 0.5���� ���� ��
                {
                    PlayerKnockBackDamage();
                    playerSc.player_Hp--;
                    playerSc.playerHp.SetPlayerHp(playerSc.player_Hp, playerSc.player_MaxHp);
                }
                ps.gameObject.transform.position = transform.position - new Vector3(0, 0.1f, 0) ;
                ps.Play();
            }
        }
        //else if (isJump)
        //{
        //    if (Player.Instance.isGround == false && Mathf.Abs(EPVecX) < 1f && verticalVelocity <= 0f) // �Ѵ� �������̰� �Ÿ��� 1���� ���� �� �ϰ����̸�
        //    {
        //        Debug.Log("���߿��� ����");
        //        PlayerKnockBackDamage();
        //    }
        //}

        if (trigger) // ȭ�� ���� ���� ���� ��
        {
            if (curTime <= 0)
            {
                verticalVelocity = jumpForce;
                isJump = true;
                stemp = true;
                curTime = coolTime;
                myAnimator.SetBool("Jump", true);
            }
            else
            {
                curTime -= Time.deltaTime;
                if(curTime <= 0.5f)
                {
                    myAnimator.SetBool("StandByJump", true);
                }
            }
        }
    }
    // ���� ���� ���� �̵��ϰ� �̵����� �� �Ÿ��� �÷��̾� ��ġ��
    // ���� �� �ְ� �������� ������ �ϰ��Ͽ� isGround�� on �� �� �ֺ��� ���� ��� �ִ� player �� enmey���� ������
}
