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

    public void SetUiItem(eQWERPSlot _value, UIItem _item)
    {
        switch (_value)
        {
            case eQWERPSlot.Q: Q = _item; break;
            case eQWERPSlot.W: W = _item; break;
            case eQWERPSlot.E: E = _item; break;
            case eQWERPSlot.R: R = _item; break;
        }
    }

    public void RemoveUiItem(eQWERPSlot _value)
    {
        switch (_value)
        {
            case eQWERPSlot.Q: Q = null; break;
            case eQWERPSlot.W: W = null; break;
            case eQWERPSlot.E: E = null; break;
            case eQWERPSlot.R: R = null; break;
        }
    }
}
