using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour
{
    private Image HpBack;
    private Image HpFront;

    void Awake()
    {
        HpBack = transform.GetChild(1).GetComponent<Image>();
        HpFront = transform.GetChild(2).GetComponent<Image>();
    }

    void Update()
    {
        checkEnemyHp();
    }


    private void checkEnemyHp()
    {

        if (HpFront.fillAmount < HpBack.fillAmount) //hp°¨¼Ò
        {
            HpBack.fillAmount -= Time.deltaTime * 0.2f;
            if (HpBack.fillAmount <= HpFront.fillAmount)
            {
                HpBack.fillAmount = HpFront.fillAmount;
            }
        }
        else if (HpBack.fillAmount < HpFront.fillAmount)
        {
            HpBack.fillAmount = HpFront.fillAmount;
        }
    }

    public void SetEnemyHp(int _curHp, int _maxHp)
    {
        HpFront.fillAmount = (float)_curHp / _maxHp;
    }
}
