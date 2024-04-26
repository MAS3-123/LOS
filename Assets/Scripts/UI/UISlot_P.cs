using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlot_P : UISlot
{
    private PassiveSkillSlot passiveSkillSlot;

    void Awake()
    {
        passiveSkillSlot = transform.GetComponentInParent<PassiveSkillSlot>();
    }
    public override void SkillSlot(eQWERPSlot _slot, UIItem _tem)
    {
        passiveSkillSlot.SetUiItem(_slot, _tem);
    }
    public override bool ShutOffMove(Transform _temTrs)
    {
        if(_temTrs.GetComponent<UISlot_P>() == null)
        {
            Debug.Log("엑티브에서 패시브 슬롯으로 옮길 수 없습니다.");
            return true;
        }
        return false;
    }
}
