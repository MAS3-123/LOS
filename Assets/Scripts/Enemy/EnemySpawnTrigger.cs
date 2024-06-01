using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public Vector3 spawnVec;
    private bool trigger = false;

    private void Awake()
    {
        spawnVec = gameObject.transform.localPosition;
    }

    public void SpwanEnemy()
    {
        if (trigger == false)
        {
            trigger = true;
            GameObject obj = Resources.Load<GameObject>($"Prefebs/Enemy/{gameObject.name}");
            GameObject obj1 = Instantiate(obj, spawnVec, Quaternion.identity, GameManager.Instance.dynamicObj.transform);
            Debug.Log(GameManager.Instance.p_fieldInEnmey);
        }
    }
}
