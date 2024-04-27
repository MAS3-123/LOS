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
    // ���� ���� ���� �̵��ϰ� �̵����� �� �Ÿ��� �÷��̾� ��ġ��
    // ���� �� �ְ� �������� ������ �ϰ��Ͽ� isGround�� on �� �� �ֺ��� ���� ��� �ִ� player �� enmey���� ������
}
