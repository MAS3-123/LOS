using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private Animator myAnimator;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] MovingStoneTrigger movingTri;
    [SerializeField] SpawnTrigger spawnTri;
    [Space]
    [SerializeField] SpriteRenderer spr;
    [Space]
    [SerializeField] public Rigidbody2D myRigid;
    [SerializeField] BoxCollider2D colliLeg;

    public InteractionObject interObj;

    Vector2 moveDir = Vector2.zero;

    [Header("스텟")]
    public float player_Speed = 5.0f;
    public float CoolDown;
    public float Damage;
    public int player_MaxHp = 10;
    public int player_MaxMp = 10;
    public int player_Hp;
    public int PATH_player_Hp;
    public int player_Mp;

    [Header("HP 연출")]
    [SerializeField] public PlayerHp playerHp;

    [Space]
    [Header("MP 연출")]
    [SerializeField] public PlayerMp playerMp;

    [Space]
    [Header(" -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - ")]
    public bool isJump = false;
    public bool dubleJump = false;
    public bool isGround = false;
    public bool basicObj = false;
    public bool enemyObj = false;
    public bool damageOn = false;

    [Space]
    public float gravity = 30.0f;
    public float jumpForce = 12.0f;
    public float groundRatio = 0.02f;
    public int fallingLimit = -15;
    public float verticalVelocity = 0f;
    public float routineF = 0f;

    public string skillLayer = string.Empty;
    public string skillTag = string.Empty;
    public string groundTag = string.Empty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        player_Hp = player_MaxHp;
        playerHp.SetPlayerHp(player_Hp, player_MaxHp); //시작시 최대 HP로 초기화
        player_Mp = player_MaxMp;
        playerMp.SetPlayerMp(player_Mp, player_MaxMp); //시작시 최대 MP로 초기화
    }

    private void OnTriggerEnter2D(Collider2D _collision) // 어떤 콜라이더와 접촉했을 때 발동되는 트리거(함수)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object")) // 접촉한 콜라이더의 레이어가 interaction object(상호작용 오브젝트)일 경우
        {
            Debug.Log("상호작용 오브젝트 접촉");
            GameObject obj = _collision.gameObject; ;// obj는 접촉한 오브젝트
            interObj = obj.GetComponent<InteractionObject>(); // stratObject는 접촉한 오브젝트의 startobj를 참조함.(이렇게 한 이유는 startobj를 가진 오브젝트가 2개인데 트리거된 오브젝트로 부터 뭔가 액션을 취하기위해 구분한 것)

            eObjectType eOType = interObj.GetComponent<ObjectType>().GetObjectType(); // 오브젝트 타입은 인스펙터에서 설정한 오브젝트 타입을 따라감.
            switch (eOType) 
            {
                case eObjectType.Basic:
                    basicObj = true; break;
                case eObjectType.Enemy:
                    enemyObj = true; break;
            }
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Spawn") // 땅에 떨어졌을 때 사용 할 스폰 트리거
        {
            Debug.Log("스폰 트리거");
            gameObject.transform.position = spawnTri.spawnVec;
            verticalVelocity = 0f;
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Enemy Spawn")
        {
            EnemySpawnTrigger eSpawnTri = _collision.gameObject.GetComponent<EnemySpawnTrigger>();
            eSpawnTri.SpwanEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        basicObj = false;
        enemyObj = false;
    }

    void Update()
    {
        if(damageOn != true)
        {
            Moving();
        }
        CheckGround();
        CheckGravity();

        InteractionObj();
        DeadPlayer();

        if(player_Hp != PATH_player_Hp)
        {
            playerHp.SetPlayerHp(player_Hp, player_MaxHp);
            PATH_player_Hp = player_Hp;
            Debug.Log("체력에 변화가 생김");
        }
    }

    private void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal"); // 가로로 움직이는 키 입력을 받아옴(좌우 에로우키, A, D)
        myRigid.velocity = new Vector2(moveDir.x * player_Speed, moveDir.y);// addforce는 질량에 의한 움직임으로 관성이 작용한다. velocity는 질량을 무시하고 직접적으로 속도를 변화하여 즉각적인 움직임이 가능하다.

        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (isGround == true && Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
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
                if (hit.transform.tag == "Move Object") // 움직이는 발판에 올라갔을 때 발판 이동방향과 속도에 맞춰 플레이어도 움직임
                {
                    routineF = movingTri.routineF;
                    myRigid.velocity = new Vector2(myRigid.velocity.x + routineF, verticalVelocity);
                }
            }
        }

    }

    private void CheckGravity()
    {
        if (isGround == false) // 공중에 떴을 경우(isGround 가 true > false 가 되자마자)
        {
            verticalVelocity -= gravity * Time.deltaTime; // verticalVelocity는 오브젝트가 y축 - 방형으로 받는 힘의 크기이고 중력값을 임의로 조정하기위해 스크립트로 작성했기에 중력을 받는 것 처럼 보이게 하려면 공중에 떴을 때부터 y축 - 방향으로 계속 힘이 가해져야 한다.
            if (verticalVelocity < fallingLimit) // 떨어지는 값이 설정한 limit값보다 작아 질 경우(떨어지는 값이 설정한 값보다 커질경우) limit값으로 보정.
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            if (isJump == true)
            {
                isJump = false;
                verticalVelocity = jumpForce; // 스페이스바를 눌렀을 때 y축 방향으로 힘을 가함.
                myAnimator.SetBool("Jump", true);
            }
            else
            {
                verticalVelocity = 0;
                //dubleJumpCount = 0;
                damageOn = false;
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("StandByJump", true);
            }
        }
        myRigid.velocity = new Vector2(myRigid.velocity.x, verticalVelocity);
    }

    private void InteractionObj() // 상호작용한 오브젝트가 갖고 있는 아이템 및 스킬 획득 함수
    {
        if (basicObj == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                interObj.trigger = true;
            }
        }
        else if (enemyObj == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("enemy");
                interObj.trigger = true;
            }
        }
    }

    private void DeadPlayer()
    {
        if(player_Hp <= 0)
        {
            transform.GetChild(0).SetParent(default);
            Destroy(gameObject);
        }
    }
}
