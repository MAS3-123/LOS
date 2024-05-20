using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVectorData
{
    public float x;
    public float y;
    public float z;
}

public class InventorySlotData
{
    public string p_Inven;
    public string a_Inven;
    public string p_SkillSlot;
    public string a_SkillSlot;
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public GameObject enemyHpBarObj;
    [SerializeField] public GameObject dynamicObj;
    [SerializeField] public GameObject interactionObj;
    [SerializeField] public GameObject playerObj;

    private Sprite spr;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GetSkill(eSkillType _skillType, Type _componentType, string _infoSkill)
    {
        InteractionObject interobj = Player.Instance.interObj; //플레이어가 상호작용한 오브젝트가 무엇인지 알기위해 플레이어를 통해 받아옴.
        GameObject item = interobj.included_Skill[0]; // 상호작용한 오브젝트가 갖고 있는 스킬 오브젝트
        
        spr = item.GetComponent<SpriteRenderer>().sprite;

        if (InventoryManager.Instance.GetItem(spr, _skillType, item.name, _infoSkill, _componentType))
        {
            return;
        }

    }
}
