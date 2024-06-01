using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMG : MonoBehaviour
{

    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button endGameButton;

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

    void Update()
    {
        //    if () 저장된 데이터가 있다면
        //    {
        //        loadGameButton.gameObject.SetActive (true);
        //    }
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
        EditorApplication.isPlaying = false;
    }
}
