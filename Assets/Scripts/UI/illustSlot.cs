using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class illustSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private Color defaultColor;
    public GameObject TMIobj;
    private RectTransform rect;

    private void Start()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        defaultColor = image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white;
        if (gameObject.transform.childCount > 0)
        {
            Transform skillObj = gameObject.transform.GetChild(0);
            GameObject obj = InventoryManager.Instance.TMI_Object;
            TMI tmi = obj.GetComponent<TMI>();

            tmi.image.sprite = skillObj.GetComponent<Image>().sprite;
            tmi.slimName.text = skillObj.name.Substring(0, skillObj.name.Length - 6);

            if (eventData.pointerEnter.gameObject != null)
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

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = defaultColor;
        Destroy(TMIobj);
    }
}
