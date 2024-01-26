using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Sprite spr;

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
    void Start()
    {

    }

    void Update()
    {

    }

    public void GetSkill()
    {
        spr = Player.Instance.skillList[0].GetComponent<SpriteRenderer>().sprite;
        if (InventoryManager.Instance.GetItem(spr))
        {
            return;
        }

    }
}
