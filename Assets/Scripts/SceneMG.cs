using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMG : MonoBehaviour
{
    public static SceneMG Instance;

    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button endGameButton;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        ClickButton();
        loadGameButton.gameObject.SetActive(false);
        if(PlayerPrefs.GetString("PlayerVec", string.Empty) != string.Empty)
        {
            loadGameButton.gameObject.SetActive(true);
        }
    }

    private void ClickButton()
    {
        loadGameButton.onClick.AddListener(() => { LoadGame(); });
        newGameButton.onClick.AddListener(() => { NewGame(); });
        endGameButton.onClick.AddListener(() => { EndGame(); });
    }

    private void LoadGame()
    {
        SaveData.Instance.p_saveOn = true;
        SceneManager.LoadScene(1);
    }

    private void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {

        }
        else if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (GameManager.Instance.p_enemyKillScore == 0)
            {
                // 천사
            }
            else if (GameManager.Instance.p_enemyKillScore == 4)
            {
                // 악마
            }
            else
            {
                // 그냥 진행
            }
        }
    }
}
