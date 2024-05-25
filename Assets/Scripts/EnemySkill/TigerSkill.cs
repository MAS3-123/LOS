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

    private bool skillReady; // true �ε��� �ٸ� �ൿ �Ұ�
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
        Rush();
        yield return new WaitForSeconds(0.15f);
        myRigid.velocity = Vector3.zero;
        rushOn = false;
        yield return new WaitForSeconds(0.3f);
        skillReady = false;
        curTime = coolTime;
        yield break;
    }


    public override void SkillOn()
    {
        if (trigger && skillReady == false)
        {
            if (isJump)
            {
                myRigid.velocity = new Vector2(EPVecX * 1.3f, verticalVelocity);
            }
            else
            {
                myRigid.velocity = new Vector2(0, verticalVelocity);
            }

            if (isGround)
            {
                verticalVelocity = 6f;
                isJump = true;
            }
        }

        if (curTime <= 0 && skillReady == false && isGround == true)
        {
            if (Mathf.Abs(EPVecX) < 5f) // �÷��̾�� ����ﶧ
            {
                StartCoroutine(RushCoroutine());
                Debug.Log("Rush");
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }

        if(rushOn && EPVecX < 1f && EPVecY < 0.5f)
        {
            playerSC.p_playerHp = -1;
            playerSC.p_playerVecX = gameObject.transform.position.x;
        }
    }

    private void Rush()
    {
        rushOn = true;
        float vec;
        if (EPVecX < 0)
        {
            vec = -50f;
        }
        else
        {
            vec = 50f;
        }
        myRigid.AddForce(new Vector2(vec, EPVecY * 3f), ForceMode2D.Impulse);
    }


    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // �Ѱ��� ����
    {
        _obj.name = "InterectionObject_Tiger Slim"; // ��ȣ�ۿ� �� ������Ʈ �̸�
        _interObj.myType = Type.GetType("TigerActiveSkill"); // �÷��̾ ��� �� ��ų ������Ʈ
        _interObj.GetComponent<itemType>().skillType = eSkillType.ActiveSkill; // ��� �� ��ų Ÿ��
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy; // ��ȣ�ۿ� �� ������Ʈ Ÿ��
        _interObj.skillTmi = "�������� ������ �����Ͽ� ??�� �������� �����ϴ�";
    }
}
