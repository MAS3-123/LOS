using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : UIItem
{
    public override void UseItem(Vector3 _vec)
    {
        Instantiate(gameObject, _vec, Quaternion.identity);
        Debug.Log("�������� ���");
    }
}
