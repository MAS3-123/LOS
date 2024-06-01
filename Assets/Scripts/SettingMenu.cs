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
    [SerializeField] GameObject illustObj;
    void Start()
    {
        illustBtn.onClick.AddListener(() => { illustButton(); });
        mainBtn.onClick.AddListener(() => { MainButton(); });
        exitBtn.onClick.AddListener(() => { ExitButton(); });
        illustObj.SetActive(false);
    }

    private void MainButton()
    {
        SceneManager.LoadScene(0);
    }

    private void illustButton()
    {
        mainBtn.gameObject.SetActive(false);
        illustBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        illustObj.SetActive(true);
    }

    private void ExitButton()
    {
        EditorApplication.isPlaying = false;
    }
}
