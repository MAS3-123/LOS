using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class UISlot_A : UISlot
{
    private ActiveSkillSlot activeSkillSlot;

    void Awake()
    {
        activeSkillSlot = transform.GetComponentInParent<ActiveSkillSlot>();
    }
    public override void SkillSlot(eQWERPSlot _slot, UIItem _tem)
    {
        activeSkillSlot.SetUiItem(_slot, _tem);
    }

    public override void MoveItem(eQWERPSlot _beforeSlot, UIItem _beforeItem)
    {
        activeSkillSlot.SetUiItem(_beforeSlot, _beforeItem);
    }
    public override bool ShutOffMove(Transform _temTrs)
    {
        if (_temTrs.GetComponent<UISlot_A>() == null)
        {
            Debug.Log("�нú꿡�� ��Ƽ�� �������� �ű� �� �����ϴ�.");
            return true;
        }
        return false;
    }
}