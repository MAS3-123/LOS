using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillSlot : MonoBehaviour
{
    [SerializeField] UIItem P;
    [Space]
    [SerializeField] Transform player;
    private int objCount = 0;

    void Update()
    {
        SkillOn();
    }

    private void SkillOn()
    {
        if(P != null && objCount == 1)
        {
            P.UseSkill(player.position);
            objCount--;
        }
    }

    public void SetUiItem(eQWERPSlot slot, UIItem _Item)
    {
        switch (slot)
        {
            case eQWERPSlot.P: P = _Item;
                 objCount++; break;
        }
    }
    public void RemoveUiItem(eQWERPSlot _value)
    {
        switch (_value)
        {
            case eQWERPSlot.P: P = null; break;
        }
    }
}
