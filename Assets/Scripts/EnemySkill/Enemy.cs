using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] public Rigidbody2D myRigid;
    [SerializeField] public BoxCollider2D colliLeg;

    public GameObject[] included_Skill;

    public bool trigger = false;
    public bool isGround = false;
    public bool isJump = false;
    //public bool Trigger { set { trigger = value; } }

    public Vector3 playerVec;
    Vector3 enemyVec;

    public float distanceGap = 0f;
    public float enemyRotate = 0f;
    public float time = 0f;

    public float gravity = 30.0f;
    public float jumpForce = 15.0f;
    public float groundRatio = 0.02f;
    public int fallingLimit = -15;
    public float verticalVelocity = 0f;

    public int count = 0;

    private void OnBecameVisible()
    {
        trigger = true;
        Debug.Log($"trigger On objectName = {gameObject.name}");

        distanceGap = 0f;
        time = 0f;
        count++;
    }
    private void OnBecameInvisible()
    {
        trigger = false;
        Debug.Log($"trigger Off objectName = {gameObject.name}");

        if (player == null) return;

        Vector3 vec = player.transform.position;
        distanceGap = enemyVec.x - vec.x;
    }

    void Update()
    {
        enemyVec = gameObject.transform.position;
        playerVec = player.transform.position;

        if (count > 0 && trigger == false)
        {
            time += Time.deltaTime;
        }

        EnemyMoving();
        CheckGround();
        CheckGravity();
        SkillOn();
    }

    private void EnemyMoving()
    {
        enemyRotate = playerVec.x - enemyVec.x;
        if(enemyRotate > 5)
        {
            enemyRotate = 5;
        }
        else if(enemyRotate < -5)
        {
            enemyRotate = -5;
        }

        if(time > 2f)
        {
            distanceGap = 0f;
            count = 0;
            Debug.Log("���� ���");
        }

        if (enemyRotate > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (enemyRotate < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //myRigid.velocity = new Vector2(0 , verticalVelocity); // 0.05�� ������ Ȯ���ϴ� ���Ƿ� ���� �� / false ������ time.deltatime �̿��� �� �� �ε巴�� ���� �ϵ��� �ؾ���.
        if (isJump)
        {
            myRigid.velocity = new Vector2(enemyRotate, verticalVelocity);
        }
        else
        {
            myRigid.velocity = new Vector2(0, verticalVelocity);
        }
    }
    private void CheckGround()
    {
        isGround = false;

        if (verticalVelocity <= 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(colliLeg.bounds.center, colliLeg.bounds.size, 0f, Vector2.down, groundRatio, LayerMask.GetMask("Ground"));

            if (hit)
            {
                isGround = true;
            }
        }

    }

    private void CheckGravity()
    {
        if (isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (verticalVelocity < fallingLimit) 
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            verticalVelocity = 0;
            isJump = false;
        }
    }

    public virtual void SkillOn()
    {

    }
    // ù ���� �� �÷��̾ �־����� ȭ�� �� �������� ���� ������ Ư�� �ð� ���� �������� ���ϸ� �̵� ����
    // ����� �����ִ� ��ų ��Ÿ�Ӹ��� ���
    // ü���� 0 ���� �� �� ��ġ�� ��ȣ�ۿ� ������ ������Ʈ ����
    // ������ ������Ʈ�� ȣ���� Enemy�� ��ų �� Ư�� ����
}
