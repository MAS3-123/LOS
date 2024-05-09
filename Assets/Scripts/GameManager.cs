using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

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

    public void GetSkill(eSkillType _skillType, Type _componentType)
    {
        InteractionObject interobj = Player.Instance.interObj; //�÷��̾ ��ȣ�ۿ��� ������Ʈ�� �������� �˱����� �÷��̾ ���� �޾ƿ�.
        GameObject item = interobj.included_Skill[0]; // ��ȣ�ۿ��� ������Ʈ�� ���� �ִ� ��ų ������Ʈ
        
        spr = item.GetComponent<SpriteRenderer>().sprite;

        if (InventoryManager.Instance.GetItem(spr, _skillType, item.name, _componentType))
        {
            return;
        }

    }
}
