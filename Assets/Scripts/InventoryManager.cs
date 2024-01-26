using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [SerializeField] public GameObject passiveInventory;
    [SerializeField] public GameObject passiveSlotGrid;
    [SerializeField] public GameObject passiveSkillSlot;
    [Space]
    [SerializeField] public GameObject activeInventory;
    [SerializeField] public GameObject activeSlotGrid;
    [SerializeField] public GameObject activeSkillSlot;
    [Space]
    [SerializeField] private GameObject objUIItem;

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
        activeInventory.SetActive(false);
        passiveInventory.SetActive(false);
        InitInventory();
    }

    private void InitInventory()
    {
        listInventory.Clear();
        listInventory.AddRange(activeSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        //if (Player.Instance.skillCF == "Active Skill")
        //{
        //    listInventory.AddRange(
        //    activeSlotGrid.transform.GetComponentsInChildren<Transform>(true)
        //    );
        //}
        //else if(Player.Instance.skillCF == "Passive Skill")
        //{
        //    listInventory.AddRange(
        //    passiveSlotGrid.transform.GetComponentsInChildren<Transform>(true)
        //    );

        //}
        listInventory.RemoveAt(0);
    }

    void Update()
    {
        callInventory();
        InventoryPos();
    }


    private void InventoryPos()
    {
        if(activeInventory.activeSelf == false)
        {
            activeInventory.transform.position = passiveInventory.transform.position;
        }
        else if(passiveInventory.activeSelf == false)
        {
            passiveInventory.transform.position = activeInventory.transform.position;
        }
    }

    private void callInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (activeInventory.activeSelf == true)
            {
                activeInventory.SetActive(false);
            }
            else if (activeInventory.activeSelf == false && passiveInventory.activeSelf == false)
            {
                activeInventory.SetActive(true);
            }
            else if(passiveInventory.activeSelf == true)
            {
                passiveInventory.SetActive(false);
            }
        }
    }

    public bool GetItem(Sprite _spr)
    {
        int slotNum = getEmptyItemSlot();
        if(slotNum == -1)
        {
            return false;
        }

        //인벤토리에 아이템을 생성
        GameObject obj = Instantiate(objUIItem, listInventory[slotNum]);
        UIItem sc = obj.GetComponent<UIItem>();
        sc.SetItem(_spr);

        return true;

    }

    private int getEmptyItemSlot()
    {
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
}
