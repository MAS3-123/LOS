using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneSkill : Enemy
{
    [SerializeField] private ParticleSystem ps;

    private bool stemp = false;
    private bool passiveOn = false;

    private float curTime = 0;
    private float coolTime = 4f;

    IEnumerator RecoveryHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            p_enemeyHp = 1;

            if(enemyHp >= 5)
            {
                passiveOn = false;
                break;
            }
        }
    }

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
                if (playerSC.isGround == true && Mathf.Abs(EPVecX) < 2f && Mathf.Abs(EPVecY) < 0.5f) // 둘다 땅에 닿아 있고 거리가 2보다 작고 높이 차이가 0.5보다 작을 때
                {
                    playerSC.p_playerVecX = gameObject.transform.position.x;
                    playerSC.p_playerHp = -2;
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
                if(p_enemeyHp <= 5)
                {
                    curTime = coolTime * 0.35f;
                }
                else
                {
                    curTime = coolTime;
                }
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

            if(p_enemeyHp < 5)
            {
                if (passiveOn == false)
                {
                    passiveOn = true;
                    StartCoroutine(RecoveryHp());
                }
            }
        }
    }

    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // 넘겨줄 정보
    {
        _obj.name = "InterectionObject_Stone Slim"; // 상호작용 할 오브젝트 이름
        _interObj.myType = Type.GetType("StonePassiveSkill"); // 플레이어가 얻게 될 스킬 컴포넌트
        _interObj.GetComponent<itemType>().skillType = eSkillType.PassiveSkill; // 얻게 될 스킬 타입
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy; // 상호작용 할 오브젝트 타입
        _interObj.skillTmi = "현재 체력이 최대 체력의 <color=#FF5733>70%</color>보다 적을 때, " +
                             "최대 체력의 <color=#FF5733>70%</color>까지 <color=#FF5733>5</color>초 마다 <color=#FF5733>1</color>씩 회복됩니다";
    }
}
