using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{
    private Animator myAnimator;
    private CircleCollider2D myCollider;

    private float myScaleX;
    private float speed = 10f;
    private float timeF = 0f;
    private float fireTime;
    private int damage = 1;

    private bool time;
    private bool localScaleCheck;
    private bool enemyCheck;
    private bool damageOn;
    private bool transformCheck;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        fireTime += Time.deltaTime;
        if (fireTime >= 0.6f)
        {
            if(localScaleCheck == false)
            {
                myScaleX = transform.parent.localScale.x;
                localScaleCheck = true;
            }
            damageOn = true;
            UseSkill();
            if (transformCheck == false)
            {
                transformCheck = true;
                GameManager.Instance.p_dynamicObj = gameObject;
                transform.SetParent(GameManager.Instance.dynamicObj.transform);
            }
        }
        DestroyObject();
    }

    private void OnTriggerStay2D(Collider2D _collision) // 쏘기전에 접촉 했을경우에도 데미지를 입히기 위해 Enter > Stay로 변경
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && damageOn)
        {
            enemyCheck = true;
            Enemy sc = _collision.gameObject.GetComponent<Enemy>();
            sc.p_enemeyHp = -damage;
            Debug.Log("FireBall : 적이랑 접촉");
        }
    }

    private void UseSkill()
    {
        transform.position += new Vector3(speed * myScaleX, 0f, 0f) * Time.deltaTime;
    }

    private void DestroyObject()
    {
        RaycastHit2D hit = Physics2D.CircleCast(myCollider.bounds.center,
            0.01f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        timeF += Time.deltaTime;

        if (timeF >= 2f)
        {
            time = true;
            Debug.Log("시간 다 됨");
        }

        if (enemyCheck || time || hit)
        {
            Destroy(gameObject);
        }
    }


}
