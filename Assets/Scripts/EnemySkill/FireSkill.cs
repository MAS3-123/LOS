using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class FireSkill : Enemy
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private GameObject skillObj;
    [SerializeField] private Transform myAim;

    public float curTime = 0;
    private float coolTime = 2f;

    public override void SkillOn()
    {
        myRigid.velocity = new Vector2(0, verticalVelocity);

        if (trigger) // 화면 내로 접근 했을 때
        {
            if (curTime <= 0)
            {
               GameObject obj = Instantiate(skillObj, myAim.position, Quaternion.identity, GameManager.Instance.dynamicObj.transform);
               obj.AddComponent<FireBall_Enemy>();
               curTime = coolTime;
            }
            else
            {
                curTime -= Time.deltaTime;
                if (curTime <= 0.5f)
                {

                }
            }
        }

    }

    public override void PlayerSkillType(GameObject _obj, InteractionObject _interObj)
    {
        _obj.name = "InterectionObject_Fire Slim";
        _interObj.myType = Type.GetType("FireActiveSkill");
        _interObj.GetComponent<itemType>().skillType = eSkillType.ActiveSkill;
        _interObj.GetComponent<ObjectType>().objectType = eObjectType.Enemy;
        _interObj.skillTmi = "직선으로 파이어볼을 발사하여 <color=#FF5733>1</color>의 피해를 입힙니다.";
    }
}
