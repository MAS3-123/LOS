using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] public Rigidbody2D myRigid;
    [SerializeField] public BoxCollider2D colliLeg;
    [SerializeField] public Animator myAnimator;

    Camera cam;

    public Player playerSc;

    public GameObject[] included_Skill;
    public GameObject[] included_myHpBar;
    public GameObject hpBarSpawnObj;
    public GameObject myHpBar;

    public bool trigger = false;
    public bool isGround = false;
    public bool isJump = false;
    //public bool Trigger { set { trigger = value; } }

    public Vector3 playerVec;
    public Vector2 EPVec;
    Vector3 enemyVec;

    public float distanceGap = 0f;
    public float EPVecX = 0f;
    public float EPVecY = 0f;
    public float time = 0f;

    public float gravity = 30.0f;
    public float jumpForce = 15.0f;
    public float groundRatio = 0.02f;
    public int fallingLimit = -15;
    public float verticalVelocity = 0f;

    public int count = 0;
    public int enemy_Hp;
    public int enemy_MaxHp = 5;

    private void OnBecameVisible()
    {
        trigger = true;
        Debug.Log($"trigger On objectName = {gameObject.name}");

        //myHpBar = Instantiate(included_myHpBar[0],gameObject.transform.position, Quaternion.identity, hpBarSpawnObj.transform);

        playerSc = player.GetComponent<Player>();

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
    private void Start()
    {
    }

    void Update()
    {
        enemyVec = gameObject.transform.position;
        if (player != null)
        {
            playerVec = player.transform.position;
        }

        if (count > 0 && trigger == false)
        {
            time += Time.deltaTime;
        }

        EnemyMoving();
        CheckGround();
        CheckGravity();
        SkillOn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Active Skill"))
        {
        }
    }

    private void EnemyMoving()
    {
        EPVecX = playerVec.x - enemyVec.x;
        EPVecY = playerVec.y - enemyVec.y;
        if(EPVecX > 5)
        {
            EPVecX = 5;
        }
        else if(EPVecX < -5)
        {
            EPVecX = -5;
        }

        if(time > 2f)
        {
            distanceGap = 0f;
            count = 0;
            Debug.Log("추적 취소");
        }

        if (EPVecX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (EPVecX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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
            myAnimator.SetBool("StandByJump", false);
            myAnimator.SetBool("Jump", false);
        }
    }

    public virtual void SkillOn()
    {

    }

    public void PlayerKnockBackDamage()
    {
        if(playerSc.damageOn != true)
        {
            if(EPVecX > 0 && EPVecX < 0.5)
            {
                EPVecX = 0.5f;
            }
            else if(EPVecX < 0 && EPVecX > -0.5)
            {
                EPVecX = -0.5f;
            }
            Debug.Log("플레이어가 근처에 있음");
            playerSc.verticalVelocity = (4f / Mathf.Abs(EPVecX));
            playerSc.myRigid.velocity = Vector3.zero;
            playerSc.myRigid.AddForce(new Vector2(100f * (1 / EPVecX), 0), ForceMode2D.Force);
            playerSc.damageOn = true;
        }
    }
    // 첫 조우 후 플레이어가 멀어지면 화면 내 범위까지 접근 하지만 특정 시간 내에 접근하지 못하면 이동 정지
    // 조우시 갖고있는 스킬 쿨타임마다 사용
    // 체력이 0 됐을 때 그 위치에 상호작용 가능한 오브젝트 생성
    // 생성된 오브젝트는 호출한 Enemy의 스킬 및 특성 보유
}
