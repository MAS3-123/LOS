using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectType
{
    Basic,
    Stage2,
    Enemy,
}

public class ObjectType : MonoBehaviour
{
    public eObjectType objectType;

    public eObjectType GetObjectType()
    {
        return objectType;
    }
}
