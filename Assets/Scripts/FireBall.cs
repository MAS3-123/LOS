using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    protected Animator myAnimator;
    protected CircleCollider2D myCollider;

    protected float myScaleX;
    protected float fireSpeed = 12f;
    private float timeF = 0f;
    private float fireTime;

    protected bool playerCheck;
    protected bool enemyCheck;
    protected bool damageOn;
    private bool time;

    public virtual void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        fireTime += Time.deltaTime;
        if(fireTime >= 0.6f)
        {
            damageOn = true;
            SummonerType();
        }
        DestroyObject();
    }

    public virtual void OnTriggerStay2D(Collider2D _collision)
    {
    }

    private void DestroyObject()
    {
        RaycastHit2D hit = Physics2D.CircleCast(myCollider.bounds.center,
            0.01f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        timeF += Time.deltaTime;

        if (timeF >= 2f)
        {
            time = true;
            Debug.Log("½Ã°£ ´Ù µÊ");
        }

        if (playerCheck || enemyCheck || time || hit)
        {
            Destroy(gameObject);
        }
    }

    public virtual void SummonerType()
    {
    }
}
