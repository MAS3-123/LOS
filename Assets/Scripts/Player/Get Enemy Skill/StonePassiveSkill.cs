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

            yield return new WaitForSeconds(finalCoolTime); // coolTime ����

            eQWERPSlot slot = gameObject.transform.parent.GetComponent<UISlot>().slot; // ������ ���� Ȯ��
            if (slot == eQWERPSlot.None) // P > None ������츦 Ȯ���ϱ� ����
            {
                StopAllCoroutines();
                passiveCount--;
                Debug.Log("ȸ�� ����");
            }

            player.playerHp_Pro = 1;
            playerAnim.SetBool("Heal", true);
            text.text = "+1";
            Debug.Log("ü���� ȸ���˴ϴ�");
            if (player.playerHp_Pro >= 10)
            {
                passiveCount--;
                break;
            }
        }
    }
     
    public override void UseSkill(Vector3 _vec)
    {
        finalCoolTime = (coolTime - subCoolTime) * multiCoolTime;
        if(passiveCount == 0 && player.playerHp_Pro < 10)
        {
            playerAnim = player.GetComponent<Animator>();
            text = player.transform.GetChild(2).GetComponent<TextMeshPro>();
            passiveCount++;
            StartCoroutine(RecoveryHp());
        }
    }
}
