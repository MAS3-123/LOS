using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{
    Vector3 vec;

    private bool enemy;
    private bool time;
    private bool isGround;

    public float timeF = 0f;

    private void Awake()
    {
        vec = Player.Instance.gameObject.transform.localScale;
    }
    void Update()
    {
        transform.position += new Vector3(8.0f * vec.x, 0f, 0f) * Time.deltaTime;
        DestroyObject();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = true;
            Debug.Log("���̶� ����");
        }
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
            Debug.Log("���̶� ����");
        }
    }

    private void DestroyObject()
    {
        timeF += Time.deltaTime;
        if (timeF > 1f)
        {
            time = true;
            Debug.Log("�ð� �� ��");
        }

        if(enemy || time || isGround)
        {
            Destroy(gameObject);
        }
    }
}
