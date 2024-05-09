using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePassiveSkill : PassiveSkill
{
    public float coolTime = 5f;
    public int count = 0;

    IEnumerator RecoveryHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolTime);
            player.player_Hp++;
            Debug.Log("체력이 회복됩니다");
            if(player.player_Hp >= 5)
            {
                passiveCount--;
                break;
            }
        }
    }
     
    public override void UseSkill(Vector3 _vec)
    {
        if(passiveCount == 0 && player.player_Hp < 5)
        {
            //GameObject obj = Resources.Load<GameObject>($"Prefebs/EnemySkill/{gameObject.name}");
            //Instantiate(obj, _vec, Quaternion.identity, obj2.transform);
            passiveCount++;
            StartCoroutine(RecoveryHp());
        }
    }
}
