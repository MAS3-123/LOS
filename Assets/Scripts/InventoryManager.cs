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

    private void InitActiveInventory()  // �κ��丮 �ʱ�ȭ �Լ�
    {
        listActiveInventory.Clear();
        listActiveInventory.AddRange(activeSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        listActiveInventory.RemoveAt(0);
    }

    private void InitPassiveInventory()  // �κ��丮 �ʱ�ȭ �Լ�
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

    public bool GetItem(Sprite _spr)
    {
        CheckInventory();
        int slotNum = getEmptyItemSlot();
        if(slotNum == -1)
        {
            return false;
        }

        //�κ��丮�� �������� ����
        GameObject obj = Instantiate(objUIItem, listInventory[slotNum]);
        UIItem sc = obj.GetComponent<UIItem>();
        sc.SetItem(_spr);

        return true;

    }

    private int getEmptyItemSlot()
    {
        CheckInventory();
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

    private void CheckInventory()
    {
        if(Player.Instance.skillCF == "Active Skill")
        {
            listInventory = listActiveInventory;
        }
        else if(Player.Instance.skillCF == "Passive Skill")
        {
            listInventory = listPassiveInventory;
        }
    }
}
