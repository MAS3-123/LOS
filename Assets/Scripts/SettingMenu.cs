using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] Button mainBtn;
    [SerializeField] Button illustBtn;
    [SerializeField] Button exitBtn;
    void Start()
    {
        illustBtn.onClick.AddListener(() => { illustButton(); });
        mainBtn.onClick.AddListener(() => { MainButton(); });
        exitBtn.onClick.AddListener(() => { ExitButton(); });
    }

    private void MainButton()
    {
        SceneManager.LoadScene(0);
    }

    private void illustButton()
    {
        Debug.Log("저장합니다.");
    }

    private void ExitButton()
    {
        EditorApplication.isPlaying = false;
    }
}
