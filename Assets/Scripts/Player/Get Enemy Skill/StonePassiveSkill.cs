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
        coolTime = 5f;
    }

    IEnumerator RecoveryHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            playerAnim.SetBool("Heal", false);

            yield return new WaitForSeconds(coolTime); // coolTime 마다

            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // 아이템 슬롯 확인
            if (slot == eQWERPSlot.None) // P > None 됐을경우를 확인하기 위해
            {
                passiveCount--;
                Debug.Log("회복 종료");
                break;
            }

            player.p_playerHp = 1;
            playerAnim.SetBool("Heal", true);
            text.text = "+1";
            Debug.Log("체력이 회복됩니다");
            if (player.p_playerHp >= player.p_playerMaxHp * 0.7)
            {
                passiveCount--;
                break;
            }
        }
    }
     
    public override void UseSkill(Vector3 _vec)
    {
        if(passiveCount == 0 && player.p_playerHp < player.p_playerMaxHp * 0.7)
        {
            playerAnim = player.GetComponent<Animator>();
            text = player.transform.GetChild(2).GetComponent<TextMeshPro>();
            passiveCount++;
            StartCoroutine(RecoveryHp());
        }
    }
}
