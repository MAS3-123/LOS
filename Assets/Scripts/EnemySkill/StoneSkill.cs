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
                if (playerSC.isGround == true && Mathf.Abs(EPVecX) < 2f && Mathf.Abs(EPVecY) < 0.5f) // �Ѵ� ���� ��� �ְ� �Ÿ��� 2���� �۰� ���� ���̰� 0.5���� ���� ��
                {
                    playerSC.p_playerVecX = gameObject.transform.position.x;
                    playerSC.p_playerHp = -2;
                }
                ps.gameObject.transform.position = transform.position - new Vector3(0, 0.1f, 0) ;
                ps.Play();
            }
        }

        if (trigger) // ȭ�� ���� ���� ���� ��
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

    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // �Ѱ��� ����
    {
        _obj.name = "InterectionObject_Stone Slim"; // ��ȣ�ۿ� �� ������Ʈ �̸�
        _interObj.myType = Type.GetType("StonePassiveSkill"); // �÷��̾ ��� �� ��ų ������Ʈ
        _interObj.GetComponent<itemType>().skillType = eSkillType.PassiveSkill; // ��� �� ��ų Ÿ��
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy; // ��ȣ�ۿ� �� ������Ʈ Ÿ��
        _interObj.skillTmi = "���� ü���� �ִ� ü���� <color=#FF5733>70%</color>���� ���� ��, " +
                             "�ִ� ü���� <color=#FF5733>70%</color>���� <color=#FF5733>5</color>�� ���� <color=#FF5733>1</color>�� ȸ���˴ϴ�";
    }
}
