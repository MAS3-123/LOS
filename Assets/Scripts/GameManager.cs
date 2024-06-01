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
        GetPlayerVector();
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
        if (SceneManager.GetActiveScene().buildIndex == 4) // 보스 씬 일때
        {
            if (enemyKillScore == 0)
            {
                // 천사
            }
            else if (enemyKillScore == 4)
            {
                // 악마
            }
            else
            {
                // 그냥 진행
            }
        }
        //else if (SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void GetPlayerVector() // 플레이어 현재 위치
    {
        PlayerPrefs.DeleteKey("PlayerVec"); // 접근시 초기화 과정
        string playerVecData = PlayerPrefs.GetString("PlayerVec", string.Empty);
        listPlayerData.Clear(); // 리스트 초기화

        PlayerVectorData data1 = new PlayerVectorData();
        data1.x = playerObj.transform.position.x;
        data1.y = playerObj.transform.position.y;
        data1.z = playerObj.transform.position.z;
        listPlayerData.Add(data1);
        playerVecData = JsonConvert.SerializeObject(listPlayerData);
        PlayerPrefs.SetString("PlayerVec", playerVecData);
    }

    private void LoadPlayerVector()
    {
        string playerVecData = PlayerPrefs.GetString("PlayerVec", string.Empty);
        listPlayerData = JsonConvert.DeserializeObject<List<PlayerVectorData>>(playerVecData);
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
