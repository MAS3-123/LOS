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
                if (playerSc.isGround == true && Mathf.Abs(EPVecX) < 2f && Mathf.Abs(EPVecY) < 0.5f) // 둘다 땅에 닿아 있고 거리가 2보다 작고 높이 차이가 0.5보다 작을 때
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
        //    if (Player.Instance.isGround == false && Mathf.Abs(EPVecX) < 1f && verticalVelocity <= 0f) // 둘다 점프중이고 거리가 1보다 작을 때 하강중이면
        //    {
        //        Debug.Log("공중에서 밟힘");
        //        PlayerKnockBackDamage();
        //    }
        //}

        if (trigger) // 화면 내로 접근 했을 때
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
    // 점프 했을 때만 이동하고 이동방향 및 거리는 플레이어 위치로
    // 점프 후 최고 지점에서 빠르게 하강하여 isGround가 on 일 때 주변에 땅에 닿아 있는 player 및 enmey에게 데미지
}
