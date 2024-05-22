using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private ParticleSystem ps;
    [SerializeField] private MovingStoneTrigger movingTri;
    [SerializeField] private SpawnTrigger spawnTri;
    [Space]
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Rigidbody2D myRigid;
    [SerializeField] private BoxCollider2D colliLeg;

    public InteractionObject interObj;

    private Vector2 moveDir = Vector2.zero;

    [Header("스텟")]
    private float playerSpeed = 5.0f;
    private float CoolDown;
    private float Damage;
    private int playerMaxHp = 10;
    private int playerMaxMp = 10;
    private int playerHp;
    private int previous_playerHp;
    private int playerMp;
    private int previous_playerMp;

    public int p_playerHp
    {
        get { return playerHp; }
        set 
        {
            if(value < 0) // 데미지 입었을 때
            {
                if (immunity == true) 
                {
                    value = 0;
                }
                else
                {
                    immunity = true;
                    StartCoroutine(ImmunityDamage());
                    Debug.Log("면역 활성화");
                    
                }
            }
            playerHp += value;
        }
    }

    public float p_playerMaxHp 
    { 
        get { return playerMaxHp; }
        set { playerMaxHp = (int)value; }
    }

    public int p_playerMp
    {
        get { return playerMp; }
        set 
        { 
            playerMp += value;
            if(playerMp < playerMaxMp && manaRecovering != true)
            {
                manaRecovering = true;
                StartCoroutine(RecoveryMana());
            }
        }
    }

    [Header("HP 연출")]
    [SerializeField] private PlayerHp playerHp_SC;

    [Space]
    [Header("MP 연출")]
    [SerializeField] private PlayerMp playerMp_SC;

    [Space]
    [Header(" -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  - ")]
    private bool isJump = false;
    private bool dubleJump = false;
    public bool isGround = false;
    private bool damageOn = false;
    private bool manaRecovering = false;
    private bool isInterObj = false;
    private bool immunity = false;

    [Space]
    public float gravity = 30.0f;
    public float jumpForce = 12.0f;
    public float groundRatio = 0.02f;
    public float verticalVelocity = 0f;
    public float routineF = 0f;
    public int fallingLimit = -15;

    private float EPVecX;

    public float p_playerVecX
    { // 넉백 당할 때 사용될 함수
        get { return EPVecX; }
        set 
        { 
            EPVecX = gameObject.transform.position.x - value; 
            damageOn = true;
            PlayerKnockBack();
        } 
    }

    private string skillLayer = string.Empty;
    private string skillTag = string.Empty;
    private string groundTag = string.Empty;

    IEnumerator RecoveryMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            playerMp += 1;
            Debug.Log("마나 지속 회복중");
            if (playerMp == playerMaxMp)
            {
                manaRecovering = false;
                Debug.Log("마나 지속 회복 끝");
                break;
            }
        }
    }

    IEnumerator ImmunityDamage()
    {
        yield return new WaitForSeconds(2f);
        immunity = false;
        Debug.Log("면역 비활성화");
        yield break;
    }

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
        playerHp = playerMaxHp;
        playerHp_SC.SetPlayerHp(playerHp, playerMaxHp); //시작시 최대 HP로 초기화
        playerMp = playerMaxMp;
        playerMp_SC.SetPlayerMp(playerMp, playerMaxMp); //시작시 최대 MP로 초기화
    }

    void Update()
    {
        if (damageOn != true)
        {
            Moving();
        }
        CheckGround();
        CheckGravity();

        InteractionObj();
        DeadPlayer();

        if (playerHp != previous_playerHp)
        {
            playerHp_SC.SetPlayerHp(playerHp, playerMaxHp);
            previous_playerHp = playerHp;
        }
        if (playerMp != previous_playerMp)
        {
            playerMp_SC.SetPlayerMp(playerMp, playerMaxMp);
            previous_playerMp = playerMp;
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision) // 어떤 콜라이더와 접촉했을 때 발동되는 트리거(함수)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object")) // 접촉한 콜라이더의 레이어가 interaction object(상호작용 오브젝트)일 경우
        {
            Debug.Log("상호작용 오브젝트 접촉");
            GameObject obj = _collision.gameObject; ;// obj는 접촉한 오브젝트
            interObj = obj.GetComponent<InteractionObject>(); // stratObject는 접촉한 오브젝트의 startobj를 참조함.(이렇게 한 이유는 startobj를 가진 오브젝트가 2개인데 트리거된 오브젝트로 부터 뭔가 액션을 취하기위해 구분한 것)

            isInterObj = true;

            interObj.myAnimator.SetBool("Interection Player", true);
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Spawn") // 땅에 떨어졌을 때 사용 할 스폰 트리거
        {
            Debug.Log("스폰 트리거");
            gameObject.transform.position = spawnTri.p_spawnVec;
            playerHp -= 3;
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
        isInterObj = false;
        if (interObj != null)
        {
            interObj.myAnimator.SetBool("Interection Player", false);
        }
    }

    private void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal"); // 가로로 움직이는 키 입력을 받아옴(좌우 에로우키, A, D)
        myRigid.velocity = new Vector2(moveDir.x * playerSpeed, moveDir.y);// addforce는 질량에 의한 움직임으로 관성이 작용한다. velocity는 질량을 무시하고 직접적으로 속도를 변화하여 즉각적인 움직임이 가능하다.

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
                if (hit.transform.tag == "Move Object_Horizotal") // 움직이는 발판에 올라갔을 때 발판 이동방향과 속도에 맞춰 플레이어도 움직임
                {
                    routineF = movingTri.routineF;
                    myRigid.velocity = new Vector2(myRigid.velocity.x + routineF, verticalVelocity);
                }
                else if(hit.transform.tag == "Move Object_Vertical")
                {
                    routineF = movingTri.routineF;
                    myRigid.velocity = new Vector2(myRigid.velocity.x, verticalVelocity + routineF);
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
                damageOn = false;
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("StandByJump", true);
            }
        }
        myRigid.velocity = new Vector2(myRigid.velocity.x, verticalVelocity);
    }

    private void InteractionObj() // 상호작용한 오브젝트가 갖고 있는 아이템 및 스킬 획득 함수
    {
        if (isInterObj && Input.GetKeyDown(KeyCode.G))
        {
            interObj.p_trigger = true;
        }
    }

    private void DeadPlayer()
    {
        if(playerHp <= 0)
        {
            transform.GetChild(0).SetParent(default); // 카메라 밖으로 뺌
            Destroy(gameObject);
        }
    }

    public void PlayerKnockBack()
    {
        if (damageOn == true)
        {
            if (EPVecX > 0 && EPVecX < 0.5) // 값 보정
            {
                EPVecX = 0.5f;
            }
            else if (EPVecX < 0 && EPVecX > -0.5) // 값 보정
            {
                EPVecX = -0.5f;
            }

            Debug.Log("적에게 데미지를 입어 넉백 됨");
            verticalVelocity = (4f / Mathf.Abs(EPVecX)); //EPVecX는 적과 내 거리 차이
            myRigid.velocity = Vector3.zero;
            myRigid.AddForce(new Vector2((1 / EPVecX) * 2f, 0), ForceMode2D.Impulse);
        }
    }
}
