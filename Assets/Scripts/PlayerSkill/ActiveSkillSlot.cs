using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ActiveSkillSlot : MonoBehaviour
{
    [SerializeField] UIItem Q;
    [SerializeField] UIItem W;
    [SerializeField] UIItem E;
    [SerializeField] UIItem R;
    [Space]
    [SerializeField] Transform player;
    [SerializeField] Transform playerAim;

    void Update()
    {
        SkillOn();
    }

    private void SkillOn()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Q.UseItem(playerAim.position);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            W.UseItem(playerAim.position);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            E.UseItem(playerAim.position);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            R.UseItem(playerAim.position);
        }
    }

    public void SetUiItem(eQWERSlot _value, UIItem _item)
    {
        switch (_value)
        {
            case eQWERSlot.Q: Q = _item; break;
            case eQWERSlot.W: W = _item; break;
            case eQWERSlot.E: E = _item; break;
            case eQWERSlot.R: R = _item; break;
        }
    }

    public void RemoveUiItem(eQWERSlot _value)
    {
        switch (_value)
        {
            case eQWERSlot.Q: Q = null; break;
            case eQWERSlot.W: W = null; break;
            case eQWERSlot.E: E = null; break;
            case eQWERSlot.R: R = null; break;
        }
    }
}
