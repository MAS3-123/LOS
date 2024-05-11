using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FireBall : MonoBehaviour
{
    private float myScaleX;
    private float timeF = 0f;

    private bool playerCheck;
    private bool time;

    Player player;

    private void Awake()
    {
        myScaleX = transform.parent.localScale.x;
        player = GameManager.Instance.playerObj.GetComponent<Player>();
    }

    void Update()
    {
        transform.position += new Vector3(8.0f * myScaleX, 0f, 0f) * Time.deltaTime;
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
        if (timeF > 1f)
        {
            time = true;
            Debug.Log("½Ã°£ ´Ù µÊ");
        }

        if (playerCheck || time)
        {
            Destroy(gameObject);
        }
    }
}
