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

    private void Update()
    {
        if (included_Skill[0] == null)
        {
            Debug.Log("아이템획득 후 오브젝트 삭제");
            Destroy(gameObject);
        }
    }

}