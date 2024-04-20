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
        GameObject item = Player.Instance.skillList[0];

        spr = item.GetComponent<SpriteRenderer>().sprite;
        LayerMask layer = item.layer; 
        string tag = item.tag;

        if (InventoryManager.Instance.GetItem(spr, layer, tag))
        {
            return;
        }

    }
}
