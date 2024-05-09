using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePassiveSkill : PassiveSkill
{
    public float coolTime = 5f;

    IEnumerator RecoveryHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolTime); // coolTime 마다
            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // 아이템 슬롯 확인
            if (slot == eQWERPSlot.None) // P > None 됐을경우를 확인하기 위해 넣음
            {
                StopAllCoroutines();
                Debug.Log("회복 종료");
            }

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
            passiveCount++;
            StartCoroutine(RecoveryHp());
        }
    }
}
