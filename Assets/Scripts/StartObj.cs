using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObj : MonoBehaviour
{
    public GameObject[] included_Skill;

    private void Update()
    {
        if (included_Skill[0] == null)
        {
            Debug.Log("������ȹ�� �� ������Ʈ ����");
            Destroy(gameObject);
        }
    }

}