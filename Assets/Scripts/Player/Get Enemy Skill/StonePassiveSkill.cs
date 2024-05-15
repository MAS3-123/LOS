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

            yield return new WaitForSeconds(coolTime); // coolTime ����

            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // ������ ���� Ȯ��
            if (slot == eQWERPSlot.None) // P > None ������츦 Ȯ���ϱ� ����
            {
                passiveCount--;
                Debug.Log("ȸ�� ����");
                break;
            }

            player.p_playerHp = 1;
            playerAnim.SetBool("Heal", true);
            text.text = "+1";
            Debug.Log("ü���� ȸ���˴ϴ�");
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
