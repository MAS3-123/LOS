using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eItemType
{
    ThrowItem,
    CunsumeItem,
}

public enum eSkillType
{
    ActiveSkill,
    PassiveSkill,
}


public class itemType : MonoBehaviour
{
    public eItemType ItemType;
    public eSkillType skillType;

    public eItemType GetItemType()
    {
        return ItemType;
    }

    public eSkillType GetSkillType()
    {
        return skillType;
    }
}
