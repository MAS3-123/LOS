using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicActiveSkill : ActiveSkill
{
    private int useMana = 1;

    private float coolTime = 1f;
    private float curTime;

    IEnumerator SkillCoolTime()
    {
        while (true)
        {
            yield return null;
            curTime -= Time.deltaTime;
            CoolAmount.fillAmount = curTime / coolTime;
            if (curTime <= 0)
            {
                curTime = 0;
                break;
            }
        }
    }

    public override void UseSkill(Vector3 _vec)
    {
        if (curTime <= 0 && player.p_playerMp - useMana >= 0)
        {
            GameObject obj = Resources.Load<GameObject>("Prefebs/BasicActiveSkill");
            Instantiate(obj, _vec, Quaternion.identity, GameManager.Instance.playerObj.transform);
            player.p_playerMp = -useMana;
            curTime = coolTime;
            StartCoroutine(SkillCoolTime());
        }
        else if (curTime > 0)
        {
            Debug.Log("��ų�� ��Ÿ�� �Դϴ�");
        }
        else if ((player.p_playerMp - useMana) < 0)
        {
            Debug.Log("������ �����Ͽ� ��ų ����� �Ұ��� �մϴ�.");
        }
    }
}
