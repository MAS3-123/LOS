using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player sc = _collision.gameObject.GetComponent<Player>();
            sc.p_playerHp = -1;
        }
    }
}
