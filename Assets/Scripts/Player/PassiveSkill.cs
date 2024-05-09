using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : UIItem
{
    protected GameObject obj2;
    protected Player player;
    public int passiveCount;

    private void Awake()
    {
        obj2 = GameManager.Instance.playerObj;
        player = obj2.GetComponent<Player>();
    }

}
