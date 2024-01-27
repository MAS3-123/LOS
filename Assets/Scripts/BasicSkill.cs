using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{

    [SerializeField] CircleCollider2D colli;

    public bool ground = false;


    void Update()
    {
        transform.position += new Vector3(1.0f, 0f, 0f) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(_collision.gameObject);
            Destroy(gameObject);
        }
    }
}
