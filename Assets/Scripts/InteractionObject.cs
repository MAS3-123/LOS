using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour // ��� ��ȣ�ۿ� ������Ʈ�� �� ��ũ��Ʈ
{
    public GameObject[] included_Skill; // ��ȣ�ۿ� ������Ʈ�� ���� ��ų ������Ʈ�� ��������
    public bool trigger = false;

    private void Update()
    {
        if (trigger)// �÷��̾ ���� Ʈ���� true
        {
            eItemType eiType = gameObject.GetComponent<itemType>().GetItemType(); // �̸� �����ص� ������ Ÿ�԰�
            eSkillType esType = gameObject.GetComponent<itemType>().GetSkillType(); // ��ų Ÿ����
            GameManager.Instance.GetSkill(eiType, esType); // ���ӸŴ����Լ��� ����
            Destroy(gameObject); //���� �� ����
        }
    }
}
