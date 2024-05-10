using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;


public class StonePassiveSkill : PassiveSkill
{
    Animator playerAnim;
    TextMeshPro text;

    public override void Awake()
    {
        base.Awake();
        coolTime = 2f;
    }

    IEnumerator RecoveryHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            playerAnim.SetBool("Heal", false);

            yield return new WaitForSeconds(finalCoolTime); // coolTime 마다

            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // 아이템 슬롯 확인
            if (slot == eQWERPSlot.None) // P > None 됐을경우를 확인하기 위해 넣음
            {
                StopAllCoroutines();
                passiveCount--;
                Debug.Log("회복 종료");
            }

            player.player_Hp++;
            Debug.Log("체력이 회복됩니다");
            playerAnim.SetBool("Heal", true);
            text.text = "+1";
            if (player.player_Hp >= 10)
            {
                passiveCount--;
                break;
            }
        }
    }
     
    public override void UseSkill(Vector3 _vec)
    {
        finalCoolTime = (coolTime - subCoolTime) * multiCoolTime;
        if(passiveCount == 0 && player.player_Hp < 10)
        {
            playerAnim = player.GetComponent<Animator>();
            float f = player.transform.GetChild(2).transform.localScale.x;
            if(player.transform.localScale.x < 0)
            {
                f = -f;
            }
            else
            {
                f = Mathf.Abs(f);
            }
            player.transform.GetChild(2).transform.localScale = new Vector3(f, player.transform.GetChild(2).transform.localScale.y, player.transform.GetChild(2).transform.localScale.z);
            text = player.transform.GetChild(2).GetComponent<TextMeshPro>();
            passiveCount++;
            StartCoroutine(RecoveryHp());
        }
    }
}
