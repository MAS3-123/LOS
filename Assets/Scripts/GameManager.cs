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

    public void GetSkill(eSkillType _skillType)
    {
        InteractionObject interobj = Player.Instance.interObj; //�÷��̾ ��ȣ�ۿ��� ������Ʈ�� �������� �˱����� �÷��̾ ���� �޾ƿ�.
        GameObject item = interobj.included_Skill[0]; // ��ȣ�ۿ��� ������Ʈ�� ���� �ִ� ��ų ������Ʈ
        
        spr = item.GetComponent<SpriteRenderer>().sprite;

        if (InventoryManager.Instance.GetItem(spr, _skillType, item.name))
        {
            return;
        }

    }
}
