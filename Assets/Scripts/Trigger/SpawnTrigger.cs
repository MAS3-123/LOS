using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private Vector3 spawnVec;

    public Vector3 p_spawnVec
    {
        get { return spawnVec; }
        set { spawnVec = value; }
    }


    private void Awake()
    {
        p_spawnVec = gameObject.transform.localPosition;
    }
}
