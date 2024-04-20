using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObjectType
{
    Basic,
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
