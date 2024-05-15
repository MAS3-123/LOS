using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Player : FireBall // �÷��̾ �� ���̾�� ��� �� ���
{
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
        myScaleX = transform.parent.localScale.x;
        fireBallDamage = 1;
    }

    public override void SummonerType()
    {
        if (transformCheck == false)
        {
            transformCheck = true;
            transform.SetParent(GameManager.Instance.dynamicObj.transform);
        }
        myAnimator.SetBool("Fire", true);
        transform.position += new Vector3(fireSpeed * myScaleX, 0f, 0f) * Time.deltaTime;
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
