using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSkillType
{
    ActiveSkill,
    PassiveSkill,
}


public class itemType : MonoBehaviour
{
    public eSkillType skillType;


    public eSkillType GetSkillType()
    {
        return skillType;
    }
}
