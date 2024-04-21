using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItem : UIItem
{
    public override void UseItem(Vector3 _vec)
    {
        GameObject obj = Resources.Load<GameObject>("Prefebs/ActiveBlueSkill");
        Instantiate(obj, _vec, Quaternion.identity);
        Debug.Log("던져지는 기능");
    }
}