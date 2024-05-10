using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour // 모든 상호작용 오브젝트에 들어갈 스크립트
{
    public Animator myAnimator;
    Rigidbody2D myRigid;
    BoxCollider2D colliLeg;
    public Type myType;

    public GameObject[] included_Skill; // 상호작용 오브젝트는 각각 스킬 오브젝트를 갖고있음 

    public bool trigger = false;
    public bool destroy = false;
    public bool isGround = false;

    private float verticalVelocity = 0f;
    private float groundRatio = 0.02f;
    private float gravity = 10f;
    protected int fallingLimit = -15;

    public string skillTmi;

    private void Awake()
    {
        ObjectType type = gameObject.GetComponent<ObjectType>();
        if(type.objectType == eObjectType.Enemy)
        {
            myRigid = GetComponent<Rigidbody2D>();
            colliLeg = GetComponent<BoxCollider2D>();
            myAnimator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (trigger)// 플레이어에 의해 트리거 true
        {
            trigger = false;
            eSkillType esType = gameObject.GetComponent<itemType>().GetSkillType(); // 스킬 타입을
            GameManager.Instance.GetSkill(esType, myType); // 게임매니져함수에 보냄
            if (destroy) Destroy(gameObject); //보낸 후 삭제
        }
        if(isGround == false && gameObject.GetComponent<ObjectType>().objectType == eObjectType.Enemy)
        {
            Spawn();
            CheckGravitiy();
        }
    }

    private void Spawn()
    {
        if (verticalVelocity <= 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(colliLeg.bounds.center, colliLeg.bounds.size, 0f, Vector2.down, groundRatio, LayerMask.GetMask("Ground"));

            if (hit)
            {
                isGround = true;

            }
        }
    }

    private void CheckGravitiy()
    {
        if(isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if(verticalVelocity < fallingLimit)
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            verticalVelocity = 0f;
        }
        myRigid.velocity = new Vector2(0f, verticalVelocity);
    }
}
