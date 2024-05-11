using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireActiveSkill : ActiveSkill
{
    private float myScaleX;
    private float timeF = 0f;

    private bool playerCheck;
    private bool time;

    public override void UseSkill(Vector3 _vec)
    {
        GameObject obj = Resources.Load<GameObject>("Prefebs/FireBall");
        Instantiate(obj, _vec, Quaternion.identity, obj2.transform);
        DestroyObject();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        Debug.Log("ÀûÀÌ¶û Á¢ÃË");
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
