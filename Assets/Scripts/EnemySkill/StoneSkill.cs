using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkill : Enemy
{
    public float coolTime = 2f;

    public override void SkillOn()
    {
        if (trigger)
        {
            if (coolTime == 2f)
            {
                verticalVelocity = jumpForce;
                isJump = true;
            }
            coolTime -= Time.deltaTime;
            if (coolTime < 0f)
            {
                coolTime = 2f;
            }
        }
    }
    // 점프 했을 때만 이동하고 이동방향 및 거리는 플레이어 위치로
    // 점프 후 최고 지점에서 빠르게 하강하여 isGround가 on 일 때 주변에 땅에 닿아 있는 player 및 enmey에게 데미지
}
