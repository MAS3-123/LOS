using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using System;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Image image;
    protected Color defaultColor;
    public GameObject TMIobj;
    protected RectTransform rect;

    public virtual void Start()
    {
        image = GetComponent<Image>(); 
        rect = GetComponent<RectTransform>(); 
        defaultColor = image.color; 
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white;
        if(gameObject.transform.childCount > 0 )
        {
            Transform skillObj = gameObject.transform.GetChild(0);
            GameObject obj = InventoryManager.Instance.TMI_Object;
            TMI tmi = obj.GetComponent<TMI>();

            tmi.image.sprite = skillObj.GetComponent<Image>().sprite;
            tmi.slimName.text = skillObj.name.Substring(0, skillObj.name.Length - 6);

            if (eventData.pointerEnter.gameObject != null )
            {
                GameObject objPointEnter = eventData.pointerEnter.gameObject;
                UIItem sc = objPointEnter.GetComponent<UIItem>();
                if (sc != null)
                {
                    tmi.tmi.text = sc.tmi;
                }
            }
            TMIobj = Instantiate(obj, gameObject.transform.position + new Vector3(-110f, 0f, 0f), Quaternion.identity, GameObject.Find("UI Canvas").gameObject.transform);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
        Destroy(TMIobj);
    }
}