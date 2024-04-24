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

    public void SetUiItem(eQWERPSlot slot, UIItem _Item)
    {
        switch(slot)
        {
            case eQWERPSlot.P: P = _Item; break;
        }
    }
}
