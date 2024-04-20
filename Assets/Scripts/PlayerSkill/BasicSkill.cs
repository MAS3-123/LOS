using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{

    [SerializeField] CircleCollider2D colli;

    void Update()
    {
        transform.position += new Vector3(4.0f, 0f, 0f) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && _collision.gameObject.tag == "Enemy")
        {
            Debug.Log("ÀûÀÌ¶û Á¢ÃË");
            Destroy(gameObject);
        }
    }
}
