using System;
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
                    playerSc.PlayerVecX_Pro = gameObject.transform.position.x;
                    playerSc.playerHp_Pro = -2;
                    playerSc.playerHp.SetPlayerHp(playerSc.playerHp_Pro, playerSc.player_MaxHp);
                }
                ps.gameObject.transform.position = transform.position - new Vector3(0, 0.1f, 0) ;
                ps.Play();
            }
        }

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

    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // 넘겨줄 정보
    {
        _obj.name = "InterectionObject_Stone Slim";
        _interObj.myType = Type.GetType("StonePassiveSkill");
        _interObj.GetComponent<itemType>().skillType = eSkillType.PassiveSkill;
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy;
        _interObj.skillTmi = "체력이 (10)이하 일 때 (2)초 마다 (1) 씩 회복됩니다!";
    }
}
