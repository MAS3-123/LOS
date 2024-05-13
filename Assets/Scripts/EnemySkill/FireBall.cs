using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FireBall : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private CircleCollider2D myCollider;

    private Player player;

    private Vector3 EPVec;
    private Vector3 myVec;

    private float myScaleX;
    private float timeF = 0f;
    private float fireTime;
    private float fireSpeed = 12f;

    private bool playerCheck;
    private bool time;
    private bool positionCheck;

    private void Start()
    {
        myScaleX = transform.parent.localScale.x;
        player = GameManager.Instance.playerObj.GetComponent<Player>();
        myVec = gameObject.transform.position;
    }

    void Update()
    {
        fireTime += Time.deltaTime;
        if(fireTime >= 0.6f)
        {
            if(positionCheck == false)
            {
                positionCheck = true;
                Vector3 playerVec = player.gameObject.transform.position;

                EPVec.x = playerVec.x  - myVec.x;
                EPVec.y = (playerVec.y - 1f) - myVec.y;
            }
            myAnimator.SetBool("Fire", true);
            transform.position += new Vector3(fireSpeed * myScaleX, EPVec.y / (Mathf.Abs(EPVec.x) * 0.2f), 0f) * Time.deltaTime;
            
        }
        DestroyObject();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerCheck = true;
            Debug.Log("ÇÃ·¹ÀÌ¾î¶û Á¢ÃË");
            player.PlayerVecX_Pro = gameObject.transform.position.x;
            player.playerHp_Pro = -1;

        }
    }

    private void DestroyObject()
    {
        timeF += Time.deltaTime;
        if (timeF >= 2f)
        {
            time = true;
            Debug.Log("½Ã°£ ´Ù µÊ");
        }

        if (playerCheck || time)
        {
            Destroy(gameObject);
        }

        RaycastHit2D hit = Physics2D.CircleCast(myCollider.bounds.center,
            0.01f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit)
        {
            Debug.Log("¶¥¿¡ ´ê¾Æ »ç¶óÁü");
            Destroy(gameObject);
        }
    }
}
