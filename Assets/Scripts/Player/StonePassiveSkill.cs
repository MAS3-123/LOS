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
            yield return new WaitForSeconds(coolTime); // coolTime ����
            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // ������ ���� Ȯ��
            if (slot == eQWERPSlot.None) // P > None ������츦 Ȯ���ϱ� ���� ����
            {
                StopAllCoroutines();
                Debug.Log("ȸ�� ����");
            }

            player.player_Hp++;
            Debug.Log("ü���� ȸ���˴ϴ�");
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
