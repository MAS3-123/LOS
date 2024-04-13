using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody2D rigid;
    public GameObject[] included_Skill;

    public bool trigger = false;
    //public bool Trigger { set { trigger = value; } }

    Vector3 vec;
    Vector3 enemyVec;

    private float distanceGap = 0f;
    public float time = 0f;

    public int count = 0;

    private void Awake()
    {
        enemyVec = gameObject.transform.position;
    }

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

        vec = player.transform.position;
        distanceGap = enemyVec.x - vec.x;

        Debug.Log($"vec = {vec}");
        Debug.Log($"Gap = {distanceGap}");
    }

    void Update()
    {
        if (count > 0 && trigger == false)
        {
            time += Time.deltaTime;
        }

        EnemyMoveing();
    }

    private void EnemyMoveing()
    {
        if(time > 2f)
        {
            distanceGap = 0f;
            count = 0;
            Debug.Log("���� ���");
        }
        rigid.velocity = new Vector2(rigid.velocity.x - distanceGap * 0.05f, rigid.velocity.y); // 0.05�� ������ Ȯ���ϴ� ���Ƿ� ���� �� / false ������ time.deltatime �̿��� �� �� �ε巴�� ���� �ϵ��� �ؾ���.
    }
    // ù ���� �� �÷��̾ �־����� ȭ�� �� �������� ���� ������ Ư�� �ð� ���� �������� ���ϸ� �̵� ����
    // ����� �����ִ� ��ų ��Ÿ�Ӹ��� ���
    // ü���� 0 ���� �� �� ��ġ�� ��ȣ�ۿ� ������ ������Ʈ ����
    // ������ ������Ʈ�� ȣ���� Enemy�� ��ų �� Ư�� ����
}
