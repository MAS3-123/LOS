using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
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

    public void GetSkill(eItemType _itemType, eSkillType _skillType)
    {
        InteractionObject interobj = Player.Instance.interObj; //플레이어가 상호작용한 오브젝트가 무엇인지 알기위해 플레이어를 통해 받아옴.
        GameObject item = interobj.included_Skill[0]; // 상호작용한 오브젝트가 갖고 있는 스킬 오브젝트
        
        spr = item.GetComponent<SpriteRenderer>().sprite;
        LayerMask layer = item.layer; 
        string tag = item.tag;

        if (InventoryManager.Instance.GetItem(spr, layer, tag, _itemType, _skillType))
        {
            return;
        }

    }
}
