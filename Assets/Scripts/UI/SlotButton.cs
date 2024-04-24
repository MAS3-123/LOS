using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void PassiveButton()
    {
        Inven.passiveBar.SetActive(true);
        Inven.activeBar.SetActive(false);
        TextMeshProUGUI text = actinven_PasButton.GetComponentInChildren<TextMeshProUGUI>();
        text.color = Color.black;
    }

    private void ActiveButton()
    {
        Inven.passiveBar.SetActive(false);
        Inven.activeBar.SetActive(true);
        TextMeshProUGUI text = pasinven_ActButton.GetComponentInChildren<TextMeshProUGUI>();
        text.color = Color.black;
    }
}
