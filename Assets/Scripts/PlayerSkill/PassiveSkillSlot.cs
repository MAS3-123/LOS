using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillSlot : MonoBehaviour
{
    [SerializeField] UIItem P;
    [Space]
    [SerializeField] Transform player;

    void Update()
    {
        SkillOn();
    }

    private void SkillOn()
    {

    }

    public void SetUiItem(ePSlot slot, UIItem _Item)
    {
        switch (slot)
        {
            case ePSlot.P: P = _Item; break;
        }
    }
}
