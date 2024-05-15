using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour // ��� ��ȣ�ۿ� ������Ʈ�� �� ��ũ��Ʈ
{
    public Animator myAnimator;
    Rigidbody2D myRigid;
    BoxCollider2D colliLeg;
    public Type myType;

    public GameObject[] included_Skill; // ��ȣ�ۿ� ������Ʈ�� ���� ��ų ������Ʈ�� �������� 

    private bool trigger = false;

    public bool p_trigger
    {
        get { return trigger; }
        set { trigger = value; }
    }

    public bool destroy = false;
    public bool isGround = false;

    private float verticalVelocity = 0f;
    private float groundRatio = 0.02f;
    private float gravity = 10f;
    protected int fallingLimit = -15;

    public string skill_Info;
    public string skillTmi
    {
        get { return skill_Info; }
        set { skill_Info = value; }
    }

    eSkillType esType;

    private void Awake()
    {
        ObjectType type = gameObject.GetComponent<ObjectType>();
        if(type.objectType == eObjectType.Enemy)
        {
            myRigid = GetComponent<Rigidbody2D>();
            colliLeg = GetComponent<BoxCollider2D>();
            myAnimator = GetComponent<Animator>();
        }
        else
        {
            esType = gameObject.GetComponent<itemType>().GetSkillType();
            if (esType == eSkillType.ActiveSkill)
            {
                skill_Info = "���� �߻� �մϴ�";
            }
            else
            {
                skill_Info = "���� ����";
                myType = Type.GetType("BasicPassiveSkill");
            }
        }
    }

    private void Update()
    {
        if (trigger)// �÷��̾ ���� Ʈ���� true
        {
            esType = gameObject.GetComponent<itemType>().GetSkillType();
            trigger = false;
            GameManager.Instance.GetSkill(esType, myType, skill_Info); // ���ӸŴ����Լ��� ����
            if (destroy) Destroy(gameObject); //���� �� ����
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
