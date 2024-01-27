using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    [SerializeField] private Button pasinven_PasButton;
    [SerializeField] private Button pasinven_ActButton;
    [Space]
    [SerializeField] private Button actinven_PasButton;
    [SerializeField] private Button actinven_ActButton;

    InventoryManager Inven;
    private void Start()
    {
        Inven = InventoryManager.Instance;
        ClickButton();
    }
    void Update()
    {
        //ActivateButton();
    }

    private void ClickButton()
    {
        pasinven_PasButton.onClick.AddListener(() => { PassiveButton(); });
        pasinven_ActButton.onClick.AddListener(() => { ActiveButton(); });
        actinven_PasButton.onClick.AddListener(() => { PassiveButton(); });
        actinven_ActButton.onClick.AddListener(() => { ActiveButton(); });
    }

    private void ActivateButton()
    {
        if (Inven.activeInventory.activeSelf == false && Inven.passiveInventory.activeSelf == true)
        {
            pasinven_ActButton.interactable = false;
            pasinven_PasButton.interactable = true;
        }
        else if(Inven.passiveInventory.activeSelf == false && Inven.activeInventory.activeSelf == true)
        {
            actinven_ActButton.interactable = true;
            actinven_PasButton.interactable = false;
        }
    }

    private void PassiveButton()
    {
        Inven.passiveBar.SetActive(true);
        Inven.activeBar.SetActive(false);
    }

    private void ActiveButton()
    {
        Inven.passiveBar.SetActive(false);
        Inven.activeBar.SetActive(true);
    }
}
