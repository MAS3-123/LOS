using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TigerSkill : Enemy
{
    [SerializeField] private ParticleSystem ps;

    public float curTime = 0;
    private float coolTime = 2f;
    private float readyTime = 0f;

    private bool skillReady; // true 인동안 다른 행동 불가
    private bool rushOn;
    public override void Awake()
    {
        base.Awake();
        curTime = coolTime;
    }

    IEnumerator RushCoroutine()
    {
        skillReady = true;
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("Rush", true);
        yield return new WaitForSeconds(0.1f);
        myAnimator.SetBool("Rush", false);
        float vec;
        if(EPVecX < 0)
        {
            vec = -4f;
        }
        else
        {
            vec = 4f;
        }
        gameObject.transform.position += new Vector3(vec, 0, 0);
        yield return new WaitForSeconds(0.5f);
        skillReady = false;
        yield break;
        // 애니메이션 말고 impulse 주고 특정 거리만큼 이동하면 포지션 고정하는걸로 정지시켜보기
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

        }

        if (curTime <= 0 && skillReady == false && isGround)
        {
            if (Mathf.Abs(EPVecX) < 3f) // 플레이어와 가까울때
            {
                StartCoroutine(RushCoroutine());
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        if(trigger && skillReady == false)
        {
            if (isGround)
            {
                verticalVelocity = 6f;
                isJump = true;
            }
        }
    }


    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // 넘겨줄 정보
    {
        _obj.name = "InterectionObject_Tiger Slim"; // 상호작용 할 오브젝트 이름
        _interObj.myType = Type.GetType("TigerActiveSkill"); // 플레이어가 얻게 될 스킬 컴포넌트
        _interObj.GetComponent<itemType>().skillType = eSkillType.ActiveSkill; // 얻게 될 스킬 타입
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy; // 상호작용 할 오브젝트 타입
        _interObj.skillTmi = "전방으로 빠르게 돌진하여 ??의 데미지를 입힙니다";
    }
}
