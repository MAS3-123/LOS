using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [SerializeField] public GameObject passiveInventory;
    [SerializeField] public GameObject passiveSlotGrid;
    [SerializeField] public GameObject passiveSkillSlot;
    [SerializeField] public GameObject passiveBar;
    [Space]
    [SerializeField] public GameObject activeInventory;
    [SerializeField] public GameObject activeSlotGrid;
    [SerializeField] public GameObject activeSkillSlot;
    [SerializeField] public GameObject activeBar;
    [Space]
    [SerializeField] private GameObject objUIItem;

    private List<Transform> listActiveInventory = new List<Transform>();
    private List<Transform> listPassiveInventory = new List<Transform>();
    private List<Transform> listInventory = new List<Transform>();


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

    private void Start()
    {
        activeBar.SetActive(false);
        passiveBar.SetActive(false);
        InitActiveInventory();
        InitPassiveInventory();
    }

    private void InitActiveInventory()  // 인벤토리 초기화 함수
    {
        listActiveInventory.Clear();
        listActiveInventory.AddRange(activeSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        listActiveInventory.RemoveAt(0);
    }

    private void InitPassiveInventory()  // 인벤토리 초기화 함수
    {
        listPassiveInventory.Clear();
        listPassiveInventory.AddRange(passiveSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        listPassiveInventory.RemoveAt(0);
    }

    void Update()
    {
        callInventory();
        InventoryPos();
    }


    private void InventoryPos()
    {
        if(activeBar.activeSelf == false)
        {
            activeBar.transform.position = passiveBar.transform.position;
        }
        else if(passiveBar.activeSelf == false)
        {
            passiveBar.transform.position = activeBar.transform.position;
        }
    }

    private void callInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (activeBar.activeSelf == true)
            {
                activeBar.SetActive(false);
            }
            else if (activeBar.activeSelf == false && passiveBar.activeSelf == false)
            {
                activeBar.SetActive(true);
            }
            else if(passiveBar.activeSelf == true)
            {
                passiveBar.SetActive(false);
            }
        }
    }

    public bool GetItem(Sprite _spr, eSkillType _sType, string _objName)
    {
        CheckInventory(_sType); // activeskill 인지 passiveskill인지 구분하는 함수

        int slotNum = getEmptyItemSlot();

        if(slotNum == -1)
        {
            return false;
        }

        //인벤토리에 아이템을 생성
        GameObject obj = Instantiate(objUIItem, listInventory[slotNum]);
        obj.name = _objName;

        switch (_sType)
        {
            case eSkillType.ActiveSkill:
                obj.AddComponent<ActiveSkill>();
                UIItem objItem = obj.GetComponent<UIItem>();
                objItem.itemSkillType = eItemSkillType.Active; break;
            case eSkillType.PassiveSkill:
                obj.AddComponent<PassiveSkill>();
                UIItem objitem = obj.GetComponent<UIItem>();
                objitem.itemSkillType = eItemSkillType.Passive; break;
        }

        UIItem sc = obj.GetComponent<UIItem>();
        sc.SetItem(_spr);

        return true;

    }

    public Transform ReturnItem(GameObject _obj)
    {
        if(_obj.GetComponent<UIItem>().itemSkillType == eItemSkillType.Active)
        {
            listInventory = listActiveInventory;
        }
        else if(_obj.GetComponent<UIItem>().itemSkillType == eItemSkillType.Passive)
        {
            listInventory = listPassiveInventory;
        }
        int slotNum = getEmptyItemSlot();
        _obj.transform.SetParent(listInventory[slotNum]);
        Transform trs = _obj.transform;
        trs = listInventory[slotNum];

        return trs;
    }

    private int getEmptyItemSlot()
    {
        //CheckInventory();
        int count = listInventory.Count;
        for(int iNum = 0; iNum < count; iNum++)
        {
            Transform trsSlot = listInventory[iNum];
            if(trsSlot.childCount == 0)
            {
                Debug.Log(iNum);
                return iNum;
            }
        }
        return -1;
    }

    private void CheckInventory(Enum _skill)
    {
        switch (_skill)
        {
            case eSkillType.ActiveSkill:
                listInventory = listActiveInventory;
                Debug.Log("Active skill"); break;
            case eSkillType.PassiveSkill:
                listInventory = listPassiveInventory;
                Debug.Log("Passive skill"); break;
        }
    }
}
