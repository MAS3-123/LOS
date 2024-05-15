using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : UIItem
{
    protected GameObject obj2;
    protected Image CoolAmount;
    protected Player player;

    private void Awake()
    {
        obj2 = GameManager.Instance.dynamicObj;
        player = GameManager.Instance.playerObj.GetComponent<Player>();
        CoolAmount = gameObject.transform.GetChild(0).GetComponent<Image>();
        CoolAmount.fillAmount = 0f;
    }
}
