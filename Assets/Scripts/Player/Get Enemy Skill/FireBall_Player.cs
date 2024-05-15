using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Player : FireBall // 플레이어가 이 파이어볼을 사용 할 경우
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

    public override void OnTriggerStay2D(Collider2D _collision) // 쏘기전에 접촉 했을경우에도 데미지를 입히기 위해 Enter > Stay로 변경
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && damageOn)
        {
            damageOn = false;
            enemyCheck = true;
            Enemy sc = _collision.gameObject.GetComponent<Enemy>();
            sc.p_enemeyHp = -fireBallDamage_Property;
            Debug.Log("FireBall : 적이랑 접촉");
        }
    }
}
