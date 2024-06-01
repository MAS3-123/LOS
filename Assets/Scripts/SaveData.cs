using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;

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
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
