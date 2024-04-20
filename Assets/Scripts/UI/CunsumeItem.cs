using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunsumeItem : UIItem
{
    public override void UseItem(Vector3 _vec)
    {
        Debug.Log("소모하는 아이템");
    }
}
