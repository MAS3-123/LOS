using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D myRigid;

    public GameObject[] included_Skill;

    public bool trigger = false;
    //public bool Trigger { set { trigger = value; } }

    Vector3 playerVec;
    Vector3 enemyVec;

    public float distanceGap = 0f;
    private float enemyRotate = 0f;
    public float time = 0f;

    public float gravity = 30.0f;
    public float jumpForce = 12.0f;
    public float groundRatio = 0.02f;
    public int fallingLimit = -15;
    private float verticalVelocity = 0f;

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
    }

    private void EnemyMoving()
    {
        enemyRotate = playerVec.x - enemyVec.x;

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

        myRigid.velocity = new Vector2(myRigid.velocity.x - 5 * distanceGap * Time.deltaTime, myRigid.velocity.y); // 0.05�� ������ Ȯ���ϴ� ���Ƿ� ���� �� / false ������ time.deltatime �̿��� �� �� �ε巴�� ���� �ϵ��� �ؾ���.
    }
    // ù ���� �� �÷��̾ �־����� ȭ�� �� �������� ���� ������ Ư�� �ð� ���� �������� ���ϸ� �̵� ����
    // ����� �����ִ� ��ų ��Ÿ�Ӹ��� ���
    // ü���� 0 ���� �� �� ��ġ�� ��ȣ�ۿ� ������ ������Ʈ ����
    // ������ ������Ʈ�� ȣ���� Enemy�� ��ų �� Ư�� ����
}
