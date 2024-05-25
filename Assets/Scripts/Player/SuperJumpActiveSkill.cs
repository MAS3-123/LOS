using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpActiveSkill : ActiveSkill
{
    private int useMana = 0;

    public override void UseSkill(Vector3 _vec)
    {
        useMana = player.p_playerMaxMp; // 마나 다 써야됨
        if (player.p_playerMp - useMana >= 0)
        {
            GameObject obj = Resources.Load<GameObject>("Prefebs/SuperJump");
            Instantiate(obj, _vec, Quaternion.identity, GameManager.Instance.playerObj.transform);
            player.p_playerMp = -useMana;
        }
    }
}
