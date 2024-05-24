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
        // �ִϸ��̼� ���� impulse �ְ� Ư�� �Ÿ���ŭ �̵��ϸ� ������ �����ϴ°ɷ� �������Ѻ���
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
            if (Mathf.Abs(EPVecX) < 3f) // �÷��̾�� ����ﶧ
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


    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj) // �Ѱ��� ����
    {
        _obj.name = "InterectionObject_Tiger Slim"; // ��ȣ�ۿ� �� ������Ʈ �̸�
        _interObj.myType = Type.GetType("TigerActiveSkill"); // �÷��̾ ��� �� ��ų ������Ʈ
        _interObj.GetComponent<itemType>().skillType = eSkillType.ActiveSkill; // ��� �� ��ų Ÿ��
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy; // ��ȣ�ۿ� �� ������Ʈ Ÿ��
        _interObj.skillTmi = "�������� ������ �����Ͽ� ??�� �������� �����ϴ�";
    }
}
