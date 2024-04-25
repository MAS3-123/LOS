using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour // 모든 상호작용 오브젝트에 들어갈 스크립트
{
    public GameObject[] included_Skill; // 상호작용 오브젝트는 각각 스킬 오브젝트를 갖고있음
    public bool trigger = false;
    public bool destroy = false;

    private void Update()
    {
        if (trigger)// 플레이어에 의해 트리거 true
        {
            trigger = false;
            eItemType eiType = gameObject.GetComponent<itemType>().GetItemType(); // 미리 지정해둔 아이템 타입과
            eSkillType esType = gameObject.GetComponent<itemType>().GetSkillType(); // 스킬 타입을
            GameManager.Instance.GetSkill(eiType, esType); // 게임매니져함수에 보냄
            if(destroy) Destroy(gameObject); //보낸 후 삭제
        }
    }
}
