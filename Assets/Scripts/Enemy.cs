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
            Debug.Log("추적 취소");
        }
        rigid.velocity = new Vector2(rigid.velocity.x - distanceGap * 0.05f, rigid.velocity.y); // 0.05는 움직임 확인하는 임의로 넣은 값 / false 됐을때 time.deltatime 이용해 좀 더 부드럽게 접근 하도록 해야함.
    }
    // 첫 조우 후 플레이어가 멀어지면 화면 내 범위까지 접근 하지만 특정 시간 내에 접근하지 못하면 이동 정지
    // 조우시 갖고있는 스킬 쿨타임마다 사용
    // 체력이 0 됐을 때 그 위치에 상호작용 가능한 오브젝트 생성
    // 생성된 오브젝트는 호출한 Enemy의 스킬 및 특성 보유
}
