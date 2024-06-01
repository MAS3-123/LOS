using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class PlayerVectorData
{
    public float x;
    public float y;
    public float z;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public GameObject enemyHpBarObj;
    [SerializeField] public GameObject interactionObj;
    [SerializeField] public GameObject playerObj;

    public GameObject dynamicObj;
    public Enemy[] fieldInEnemy;

    private List<PlayerVectorData> listPlayerData = new List<PlayerVectorData>();

    public int p_fieldInEnmey
    {
        get
        {
            fieldInEnemy = dynamicObj.GetComponentsInChildren<Enemy>();
            return fieldInEnemy.Length;
        }
    }

    private int enemyKillScore;

    public int p_enemyKillScore
    {
        get
        {
            return enemyKillScore;
        }
        set
        {
            enemyKillScore += value;
        }
    }

    private bool saveOn;

    public bool p_saveOn
    {
        get { return saveOn; }
        set
        {
            saveOn = value;
        }
    }
    private Sprite spr;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        if(SaveData.Instance != null)
        {
            p_saveOn = SaveData.Instance.p_saveOn;
        }
    }

    private void Start()
    {
        if (saveOn)
        {
            LoadPlayerVector();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dynamicObj = GameObject.Find("DynamicObject");
    }

    public void GetPlayerVector() // �÷��̾� ���� ��ġ
    {
        PlayerPrefs.DeleteKey("PlayerVec"); // ���ٽ� �ʱ�ȭ ����
        listPlayerData.Clear(); // ����Ʈ �ʱ�ȭ

        PlayerVectorData data = new PlayerVectorData();
        data.x = playerObj.transform.position.x;
        data.y = playerObj.transform.position.y;
        data.z = playerObj.transform.position.z;
        listPlayerData.Add(data);
        string playerVecData = JsonConvert.SerializeObject(listPlayerData);
        PlayerPrefs.SetString("PlayerVec", playerVecData);
    }

    private void LoadPlayerVector()
    {
        string playerVecData = PlayerPrefs.GetString("PlayerVec", string.Empty);
        listPlayerData = JsonConvert.DeserializeObject<List<PlayerVectorData>>(playerVecData);
        float x = listPlayerData[0].x;
        float y = listPlayerData[0].y;
        float z = listPlayerData[0].z;
        playerObj.transform.position = new Vector3(x, y, z);
    }

    public void GetSkill(eSkillType _skillType, Type _componentType, string _infoSkill)
    {
        InteractionObject interobj = Player.Instance.interObj; //�÷��̾ ��ȣ�ۿ��� ������Ʈ�� �������� �˱����� �÷��̾ ���� �޾ƿ�.
        GameObject item = interobj.included_Skill[0]; // ��ȣ�ۿ��� ������Ʈ�� ���� �ִ� ��ų ������Ʈ
        spr = item.GetComponent<SpriteRenderer>().sprite;

        if (InventoryManager.Instance.GetItem(spr, _skillType, item.name, _infoSkill, _componentType))
        {
            return;
        }

    }
}
