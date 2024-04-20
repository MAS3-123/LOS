using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public Vector3 spawnVec;

    private void Awake()
    {
        spawnVec = gameObject.transform.localPosition;
    }
}
