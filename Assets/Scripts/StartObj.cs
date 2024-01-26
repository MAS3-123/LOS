using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObj : MonoBehaviour
{
    public static StartObj Instance;

    public GameObject[] included_Skill;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update() // �ϼ��Ȱžƴ� �ٲ����
    {
        if(CheckIncludeSkill() == false)
        {
            Destroy(gameObject);
        }
    }

    private bool CheckIncludeSkill()
    {
        if(included_Skill != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}