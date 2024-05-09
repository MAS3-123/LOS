using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : UIItem
{
    GameObject obj2;

    private void Awake()
    {
        obj2 = GameManager.Instance.dynamicObj;
    }
    public override void UseSkill(Vector3 _vec)
    {
        GameObject obj = Resources.Load<GameObject>($"Prefebs/{gameObject.name}");
        Instantiate(obj, _vec, Quaternion.identity, obj2.transform);
    }
}