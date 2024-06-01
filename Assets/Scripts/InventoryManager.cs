using Newtonsoft.Json;
using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventorySlotData
{
    public string pInvenSlot;
    public string aInvenSlot;
    public string pSkillSlot;
    public string aSkillSlot;
}

public class ItemData
{
    public string spriteName;
    public string eSkillType;
    public string objName;
    public string objTmi;
    public string objComponentType;
}

public class SlotData
{
    public string slotName;
}


public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    [SerializeField] public GameObject passiveInventory;
    [SerializeField] public GameObject PassiveSlotGrid;
    [SerializeField] public GameObject PassiveSkillSlot;
    [SerializeField] public GameObject passiveBar;
    [Space]
    [SerializeField] public GameObject activeInventory;
    [SerializeField] public GameObject ActiveSlotGrid;
    [SerializeField] public GameObject ActiveSkillSlot;
    [SerializeField] public GameObject activeBar;
    [Space]
    [SerializeField] private GameObject objUIItem;
    [SerializeField] public GameObject TMI_Object;
    [SerializeField] private GameObject settingMenu;
    [Space]
    [SerializeField] private GameObject ActiveSkillillust;
    [SerializeField] private GameObject PassiveSkillillust;
    [SerializeField] private GameObject Slimillust;

    private List<Transform> listActiveInventory = new List<Transform>();
    private List<Transform> listPassiveInventory = new List<Transform>();
    private List<Transform> listInventory = new List<Transform>();
    private List<InventorySlotData> listInventoryData = new List<InventorySlotData>();

    private List<ItemData> listItemData = new List<ItemData>();
    private List<ItemData> listItemInfo = new List<ItemData>();
    private List<SlotData> listSlotName = new List<SlotData>();

    private string[] keyName_Item = new string[4];
    private GameObject[] keyName_Slot = new GameObject[4];

    private bool saveOn;

    public bool p_saveOn
    {
        get { return saveOn; }
        set
        {
            saveOn = value;
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        if (SaveData.Instance != null)
        {
            p_saveOn = SaveData.Instance.p_saveOn;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void Start()
    {
        settingMenu.SetActive(false);
        activeBar.SetActive(false);
        passiveBar.SetActive(false);
        InitActiveInventory();
        InitPassiveInventory();

        GameObject[] obj = { ActiveSlotGrid, PassiveSlotGrid, ActiveSkillSlot, PassiveSkillSlot };
        for (int i = 0; i < obj.Length; i++)
        {
            keyName_Slot[i] = obj[i];
            keyName_Item[i] = obj[i].name + "_Item";
        }

        if (p_saveOn)
        {
            LoadData();
        }

        string itemInfo = PlayerPrefs.GetString("Item_Infomation", string.Empty);
        if (itemInfo != string.Empty)
        {
            List<ItemData> itemDatas = JsonConvert.DeserializeObject<List<ItemData>>(itemInfo);
            for (int i = 0; i < itemDatas.Count; i++)
            {
                
            }
        }
    }

    void Update()
    {
        callInventory();
        InventoryPos();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        listSlotName.Clear();
        listItemData.Clear();

        for(int slotNum = 0;slotNum < keyName_Slot.Length; slotNum++)
        {
            string slotName = PlayerPrefs.GetString(keyName_Slot[slotNum].name, string.Empty);
            string itemData = PlayerPrefs.GetString(keyName_Slot[slotNum].name + "_Item", string.Empty);

            if (slotName != string.Empty)
            {
                listSlotName = JsonConvert.DeserializeObject<List<SlotData>>(slotName);
                listItemData = JsonConvert.DeserializeObject<List<ItemData>>(itemData);
                UISlot[] slot = keyName_Slot[slotNum].GetComponentsInChildren<UISlot>(true);
                for(int sNum = 0; sNum < listSlotName.Count; sNum++)
                {
                    for (int itemNum = 0; itemNum < slot.Length; itemNum++)
                    {
                        if (slot[itemNum].gameObject.name == listSlotName[sNum].slotName)
                        {
                            Sprite spr = Resources.Load<Sprite>($"Sprite/{listItemData[sNum].spriteName}");
                            eSkillType skillType = (eSkillType)Enum.Parse(typeof(eSkillType), listItemData[sNum].eSkillType);
                            string objName = listItemData[sNum].objName;
                            string tmi = listItemData[sNum].objTmi;
                            Type componentType = Type.GetType(listItemData[sNum].objComponentType);
                            CheckInventory(skillType);
                            getEmptyItemSlot();
                            GameObject obj = Instantiate(objUIItem, slot[itemNum].gameObject.transform);

                            InstantiateItem(obj, spr, skillType, objName, tmi, componentType);
                            Debug.Log($"{listSlotName[sNum].slotName} 여기에 아이템 있었음");
                        }
                    }
                }
            }
        }
    }

    private void LoadSlotData(string _slotName)
    {
        listSlotName.Clear();
        string slotName = PlayerPrefs.GetString(_slotName, string.Empty);
        if (slotName == string.Empty)
        {
            Debug.Log($"slot >>{_slotName}<< 의 데이터는 비어있습니다.");
        }
        else
        {
            listSlotName = JsonConvert.DeserializeObject<List<SlotData>>(slotName);
            for (int i = 0; i < listSlotName.Count; i++)
            {
                Debug.Log($"slot >>{_slotName}<< 의 데이터는 {listSlotName[i].slotName}.");
            }
        }
    }
    private void LoadItemData(string _objName)
    {
        listItemData.Clear();
        string itemData = PlayerPrefs.GetString(_objName, string.Empty);
        if (itemData == string.Empty)
        {
            Debug.Log($">>{_objName}<< 의 데이터는 비어있습니다.");
        }
        else
        {
            listItemData = JsonConvert.DeserializeObject<List<ItemData>>(itemData);
            for (int i = 0; i < listItemData.Count; i++)
            {
                Debug.Log($">>{_objName}<< 의 데이터는 {listItemData[i].objName} ,{listItemData[i].spriteName} ,{listItemData[i].objTmi}, {listItemData[i].eSkillType}" +
                $", {listItemData[i].objComponentType} .");
            }
        }
    }

    public void SaveSlotData() // 슬롯 정보를 저장
    {
        for (int iNum = 0; iNum < keyName_Slot.Length; iNum++)
        {
            PlayerPrefs.DeleteKey(keyName_Slot[iNum].name); // 각 슬롯의 부모의 이름을 Keyname으로 정할거 (저장 할 때마다 삭제 안하면 중복되서 쌓임)
            UIItem[] uiItems = keyName_Slot[iNum].transform.GetComponentsInChildren<UIItem>(true); // 상위 부모의 자식으로부터 아이템 컴포넌트를 갖고있는 녀석들을 분류
            if (uiItems.Length > 0) // 슬롯에 아이템이 존재 할 경우
            {
                listSlotName.Clear(); // 재사용 할 것임으로 list 접근시 매번 초기화
                listItemData.Clear();
                for (int i = 0; i < uiItems.Length; i++) // 아이템을 갖고있는 만큼 반복
                {
                    SlotData slotData = new SlotData();
                    slotData.slotName = uiItems[i].transform.parent.name;
                    listSlotName.Add(slotData); // 아이템이 존재하는 슬롯의 이름을 알기위해 아이템의 부모의 이름을 리스트에 저장

                    string itemInfo = PlayerPrefs.GetString("Item_Infomation", string.Empty);

                    if (itemInfo != string.Empty)
                    {
                        List<ItemData> itemDatas = JsonConvert.DeserializeObject<List<ItemData>>(itemInfo);
                        for (int j = 0; j < itemDatas.Count; j++) // 게임도중 획득한 아이템들중에
                        {
                            if (uiItems[i].gameObject.name == itemDatas[j].objName) // 저장하려는 아이템 이름과 획득한 아이템 이름이 같다면 획득한 아이템 정보를 저장
                            {
                                ItemData data = new ItemData();
                                data.spriteName = itemDatas[j].spriteName;
                                data.eSkillType = itemDatas[j].eSkillType;
                                data.objName = itemDatas[j].objName;
                                data.objTmi = itemDatas[j].objTmi;
                                data.objComponentType = itemDatas[j].objComponentType;

                                listItemData.Add(data);
                            }
                        }
                        string itemData = JsonConvert.SerializeObject(listItemData);
                        PlayerPrefs.SetString(keyName_Item[iNum], itemData); // slot의 부모 이름을 keyName_Item으로 아이템의 정보를 저장
                        Debug.Log(PlayerPrefs.GetString(keyName_Item[iNum]));
                    }
                }
                string slotName = JsonConvert.SerializeObject(listSlotName);
                PlayerPrefs.SetString(keyName_Slot[iNum].name, slotName); // slot의 부모 이름을 keyName으로 아이템이 있었던 slot의 이름을 저장
            }
        }
    }

    private void SaveItemInfo(Sprite _spr, eSkillType _sType, string _objName, string _tmi, Type _componentType)
    {
        string itemInfo = PlayerPrefs.GetString("Item_Infomation", string.Empty);
        if (itemInfo != string.Empty)
        {
            List<ItemData> itemDatas = JsonConvert.DeserializeObject<List<ItemData>>(itemInfo);
            for (int i = 0; i < itemDatas.Count; i++)
            {
                if (_objName == itemDatas[i].objName) // 획득한 아이템의 정보가 이미 있을경우
                {
                    Debug.Log("이미 정보가 저장된 아이템");
                    //Debug.Log(PlayerPrefs.GetString("Item_Infomation"));
                    return;
                }
            }
            listItemInfo = JsonConvert.DeserializeObject<List<ItemData>>(itemInfo);
        }

        ItemData data = new ItemData();
        data.spriteName = _spr.name;
        data.eSkillType = _sType.ToString();
        data.objName = _objName;
        data.objTmi = _tmi;
        data.objComponentType = _componentType.ToString();

        listItemInfo.Add(data);

        string itemData = JsonConvert.SerializeObject(listItemInfo);
        PlayerPrefs.SetString("Item_Infomation", itemData);
        //List<ItemData> itemInfomation = JsonConvert.DeserializeObject<List<ItemData>>(itemData);
        LoadIlluste(data);
    }

    private void LoadIlluste(ItemData data)
    {
        switch (data.eSkillType)
        {
            case "ActiveSkill":
                Debug.Log("엑티브 스킬 도감");
                RectTransform myTrs = ActiveSkillillust.GetComponent<RectTransform>();
                RectTransform[] rect = ActiveSkillillust.GetComponentsInChildren<RectTransform>().Where(t => t != myTrs).ToArray();
                GameObject obj1 = Instantiate(objUIItem, rect[0]);
                GameObject tmiObj = TMI_Object;
                TMI tmi = tmiObj.GetComponent<TMI>();
                Sprite spr = Resources.Load<Sprite>($"Sprite/{data.spriteName}");

                Destroy(obj1.transform.GetChild(0).gameObject);

                obj1.GetComponent<Image>().sprite = spr;
                tmi.slimName.text = data.objName;
                tmi.image.sprite = spr;
                tmi.tmi.text = data.objTmi;

                break;
            case "PassiveSkill":
                Debug.Log("패시브 스킬 도감");
                GameObject obj2 = Instantiate(objUIItem, PassiveSkillillust.transform);
                break;
        }
    }

    private void InitActiveInventory()  // 인벤토리 초기화 함수
    {
        listActiveInventory.Clear();
        listActiveInventory.AddRange(ActiveSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        listActiveInventory.RemoveAt(0);
    }

    private void InitPassiveInventory()  // 인벤토리 초기화 함수
    {
        listPassiveInventory.Clear();
        listPassiveInventory.AddRange(PassiveSlotGrid.transform.GetComponentsInChildren<Transform>(true));
        listPassiveInventory.RemoveAt(0);
    }


    private void InventoryPos()
    {
        if (activeBar.activeSelf == false)
        {
            activeBar.transform.position = passiveBar.transform.position;
        }
        else if (passiveBar.activeSelf == false)
        {
            passiveBar.transform.position = activeBar.transform.position;
        }
    }

    private void callInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (activeBar.activeSelf == true && settingMenu.activeSelf == false)
            {
                activeBar.SetActive(false);
            }
            else if (activeBar.activeSelf == false && passiveBar.activeSelf == false && settingMenu.activeSelf == false)
            {
                activeBar.SetActive(true);
            }
            else if (passiveBar.activeSelf == true && settingMenu.activeSelf == false)
            {
                passiveBar.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeBar.activeSelf == true || passiveBar.activeSelf == true)
            {
                activeBar.SetActive(false);
                passiveBar.SetActive(false);
            }
            else if (settingMenu.activeSelf == false)
            {
                settingMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else if (settingMenu.activeSelf == true)
            {
                settingMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public bool GetItem(Sprite _spr, eSkillType _sType, string _objName, string _tmi, Type _componentType)
    {
        SaveItemInfo(_spr, _sType, _objName, _tmi, _componentType);
        CheckInventory(_sType); // activeskill 인지 passiveskill인지 구분하는 함수
        int slotNum = getEmptyItemSlot();

        if (slotNum == -1)
        {
            return false;
        }

        GameObject obj = Instantiate(objUIItem, listInventory[slotNum]);
        InstantiateItem(obj, _spr, _sType, _objName, _tmi, _componentType);

        SaveSlotData();

        return true;
    }

    private void InstantiateItem(GameObject obj, Sprite _spr, eSkillType _sType, string _objName, string _tmi, Type _componentType)
    {
        obj.name = _objName;
        UIItem objItem = null;
        obj.AddComponent(_componentType);

        objItem = obj.GetComponent<UIItem>();
        objItem.tmi = _tmi;
        objItem.itemSkillType = _sType == eSkillType.ActiveSkill ? eItemSkillType.Active : eItemSkillType.Passive;

        UIItem sc = obj.GetComponent<UIItem>();
        sc.SetItem(_spr);
    }

    public Transform ReturnItem(GameObject _obj)
    {
        if (_obj.GetComponent<UIItem>().itemSkillType == eItemSkillType.Active)
        {
            listInventory = listActiveInventory;
        }
        else if (_obj.GetComponent<UIItem>().itemSkillType == eItemSkillType.Passive)
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
        int count = listInventory.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            Transform trsSlot = listInventory[iNum];
            if (trsSlot.childCount == 0)
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
