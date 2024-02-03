using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMp : MonoBehaviour
{
    private Image MpBack;
    private Image MpFront;

    void Awake()
    {
        MpBack = transform.GetChild(1).GetComponent<Image>();
        MpFront = transform.GetChild(2).GetComponent<Image>();
    }

    void Update()
    {
        checkPlayerMp();
    }


    private void checkPlayerMp()
    {

        if (MpFront.fillAmount < MpBack.fillAmount) //hp°¨¼Ò
        {
            MpBack.fillAmount -= Time.deltaTime * 0.2f;
            if (MpBack.fillAmount <= MpFront.fillAmount)
            {
                MpBack.fillAmount = MpFront.fillAmount;
            }
        }
        else if (MpBack.fillAmount < MpFront.fillAmount)
        {
            MpBack.fillAmount = MpFront.fillAmount;
        }
    }

    public void SetPlayerMp(int _curHp, int _maxHp)
    {
        MpFront.fillAmount = (float)_curHp / _maxHp;
    }
}
