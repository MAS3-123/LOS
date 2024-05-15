using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Enemy : FireBall // ���� �� ���̾�� ��� �� ���
{
    private Player player;

    private Vector3 EPVec;
    private Vector3 myVec;

    private bool positionCheck;

    public override void Start()
    {
        base.Start();
        player = GameManager.Instance.playerObj.GetComponent<Player>();
        myVec = gameObject.transform.position;
        if ((player.transform.position.x - gameObject.transform.position.x) < 0)
        {
            myScaleX = -1f;
        }
        else if ((player.transform.position.x - gameObject.transform.position.x) > 0)
        {
            myScaleX = 1f;
        }
    }

    public override void SummonerType()
    {
        if (positionCheck == false)
        {
            positionCheck = true;
            Vector3 playerVec = player.gameObject.transform.position;

            EPVec.x = playerVec.x - myVec.x;
            EPVec.y = (playerVec.y - 1f) - myVec.y;
        }
        myAnimator.SetBool("Fire", true);
        transform.position += new Vector3(fireSpeed * myScaleX, EPVec.y / (Mathf.Abs(EPVec.x) * 0.2f)/* �Ÿ��� ���� �߻� ���� �޶����� ���� */, 0f) * Time.deltaTime;
    }

    public override void OnTriggerStay2D(Collider2D _collision) // ������� ���� ������쿡�� �������� ������ ���� Enter > Stay�� ����
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player") && damageOn)
        {
            damageOn = false;
            playerCheck = true;
            Debug.Log("FireBall : �÷��̾�� ����");
            player.p_playerVecX = gameObject.transform.position.x;
            player.p_playerHp = -1;
        }
    }
}
