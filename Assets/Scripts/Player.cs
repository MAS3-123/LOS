using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] MovingStoneTrigger movingTri;
    [SerializeField] SpawnTrigger spawnTri;
    [SerializeField] StartObj startObject;
    [Space]
    [SerializeField] SpriteRenderer spr;
    [Space]
    [SerializeField] Rigidbody2D myRigid;
    [SerializeField] BoxCollider2D colliLeg;

    Vector2 moveDir = Vector2.zero;

    public GameObject[] skill;
    public List<GameObject> skillList = new List<GameObject>();

    [Header("스텟")]
    public float player_Speed = 5.0f;
    public float CoolDown;
    public float Damage;
    public int player_MaxHp = 10;
    public int player_MaxMp = 10;
    private int player_Hp;
    private int player_Mp;

    [Header("HP 연출")]
    [SerializeField] private PlayerHp playerHp;

    [Space]
    [Header("MP 연출")]
    [SerializeField] private PlayerMp playerMp;

    [Space]
    [Header(" -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - ")]
    public bool isJump = false;
    public bool isGround = false;
    public bool basicObj = false;
    public bool enemyObj = false;
    public bool enemy = false;

    [Space]
    public float gravity = 30.0f;
    public float jumpForce = 12.0f;
    public float groundRatio = 0.02f;
    public int fallingLimit = -15;
    private float verticalVelocity = 0f;
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

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object") && _collision.gameObject.tag == "Basic Object") // 처음 상호작용 하게 될 오브젝트
        {
            basicObj = true;
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object") && _collision.gameObject.tag == "Enemy Object") // 적 처치시 상호작용 하게 될 오브젝트
        {
            enemyObj = true;
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Enmey") && _collision.gameObject.tag == "Enemy")
        {
            enemy = true;
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Spawn")
        {
            gameObject.transform.position = spawnTri.spawnVec;
            verticalVelocity = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        basicObj = false;
        enemyObj = false;
    }

    private void Start()
    {
        player_Hp = player_MaxHp;
        playerHp.SetPlayerHp(player_Hp, player_MaxHp); //시작시 최대 HP로 초기화
        player_Mp = player_MaxMp;
        playerMp.SetPlayerMp(player_Mp, player_MaxMp); //시작시 최대 MP로 초기화
    }

    void Update()
    {
        Moving();

        CheckGround();
        CheckGravity();

        InteractionObj();
    }

    private void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        myRigid.velocity = new Vector2(moveDir.x * player_Speed, moveDir.y);

        if(moveDir.x > 0)
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

        if(verticalVelocity <= 0)
        {
            RaycastHit2D hit = Physics2D.BoxCast(colliLeg.bounds.center, colliLeg.bounds.size, 0f, Vector2.down, groundRatio, LayerMask.GetMask("Ground"));

            if (hit)
            {
                isGround = true;
                if(hit.transform.tag == "Move Object") // 움직이는 발판에 올라갔을 때 발판 이동방향과 속도에 맞춰 플레이어도 움직임
                {
                    routineF = movingTri.routineF;
                    myRigid.velocity = new Vector2(myRigid.velocity.x + routineF, verticalVelocity);
                }
            }
        }

    }

    private void CheckGravity()
    {
        if(isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if(verticalVelocity < fallingLimit)
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            if(isJump == true) 
            {
                isJump = false;
                verticalVelocity = jumpForce;
            }
            else
            {
                verticalVelocity = 0;
            }
        }

        myRigid.velocity = new Vector2(myRigid.velocity.x, verticalVelocity);
    }

    private void InteractionObj() // 상호작용한 오브젝트가 갖고 있는 아이템 및 스킬 획득 함수
    {
        if(basicObj == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                skill[0] = startObject.included_Skill[0];
                startObject.included_Skill[0] = null;
                skillList.Add(skill[0]);
                Skillclassification();
            }

            if (skill[0] != null)
            {
                GameManager.Instance.GetSkill();
                skill[0] = null;
            }
        }

        else if(enemyObj == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("enemy");
            }
        }
    }


    private void Skillclassification() // 스킬 분류 함수
    {
        if (skillList[0].layer == LayerMask.NameToLayer("Active Skill"))
        {
            skillLayer = "Active Skill";
        }
        else if (skillList[0].layer == LayerMask.NameToLayer("Passive Skill"))
        {
            skillLayer = "Passive Skill";
        }
        skillTag = skillList[0].tag;
    }
}
