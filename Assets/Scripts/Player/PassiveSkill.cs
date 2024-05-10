using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : UIItem
{
    protected GameObject obj2;
    protected Player player;
    public int passiveCount;
    public float coolTime;
    public float subCoolTime = 0f;
    public float multiCoolTime = 1f;
    public float finalCoolTime = 0f;

    public virtual void Awake()
    {
        obj2 = GameManager.Instance.playerObj;
        player = obj2.GetComponent<Player>();
    }

}
