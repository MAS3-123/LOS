using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Player : FireBall // �÷��̾ �� ���̾�� ��� �� ���
{//�θ�� �ڽ� ��� RigidBody component�� ������� ������Ʈ�� �θ� �ڽİ� ����Ǿ�������, �ڽ��� �θ� position�� ������ �ʴ´�. localScale�� �����.
    private bool transformCheck;

    private int fireBallDamage;

    public int fireBallDamage_Property 
    { 
        get { return fireBallDamage; }
        set { fireBallDamage = value; }
    }

    public override void Start()
    {
        base.Start();
        //myScaleX = transform.parent.localScale.x;
        fireBallDamage = 1;
    }

    public override void SummonerType()
    {
        myAnimator.SetBool("Fire", true);
        transform.position += new Vector3(fireSpeed * myScaleX, 0f, 0f) * Time.deltaTime;
        if (transformCheck == false) //�߻� �� �θ� transform�� Player > DynamicObject
        {
            transformCheck = true;
            myScaleX = transform.parent.localScale.x;
            transform.SetParent(GameManager.Instance.dynamicObj.transform);
        }
    }

    public override void OnTriggerStay2D(Collider2D _collision) // ������� ���� ������쿡�� �������� ������ ���� Enter > Stay�� ����
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && damageOn)
        {
            damageOn = false;
            enemyCheck = true;
            Enemy sc = _collision.gameObject.GetComponent<Enemy>();
            sc.p_enemeyHp = -fireBallDamage_Property;
            Debug.Log("FireBall : ���̶� ����");
        }
    }
}
