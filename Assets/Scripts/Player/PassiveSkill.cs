using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : UIItem
{
    protected GameObject obj2;
    protected Player player;
    public int passiveCount;
    protected float coolTime;
    protected float subCoolTime = 0f;
    protected float multiCoolTime = 1f;
    protected float finalCoolTime = 0f;

    public float coolTimeProperty
    {
        get { return  coolTime; }
        set { coolTime = value; }
    }

    public float subCoolTimeProperty
    {
        get { return subCoolTime; }
        set { subCoolTime = value; }
    }

    public float multiCoolTimeProperty
    {
        get { return multiCoolTime; }
        set { multiCoolTime = value; }
    }

    public float finalCoolTimeProperty
    {
        get { return finalCoolTime; }
        set { finalCoolTime = value; }
    }

    public virtual void Awake()
    {
        obj2 = GameManager.Instance.playerObj;
        player = obj2.GetComponent<Player>();
    }

}
