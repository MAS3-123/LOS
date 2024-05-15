using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkill : UIItem
{
    protected GameObject obj2;
    protected Player player;
    protected Image CoolAmount;
    public int passiveCount;
    protected float coolTime;

    public virtual void Awake()
    {
        obj2 = GameManager.Instance.playerObj;
        player = obj2.GetComponent<Player>();
        CoolAmount = gameObject.transform.GetChild(0).GetComponent<Image>();
        CoolAmount.fillAmount = 0f;
    }

}
