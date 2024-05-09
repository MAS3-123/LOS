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

    [Header("����")]
    public float player_Speed = 5.0f;
    public float CoolDown;
    public float Damage;
    public int player_MaxHp = 10;
    public int player_MaxMp = 10;
    public int player_Hp;
    public int PATH_player_Hp;
    public int player_Mp;

    [Header("HP ����")]
    [SerializeField] public PlayerHp playerHp;

    [Space]
    [Header("MP ����")]
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
        playerHp.SetPlayerHp(player_Hp, player_MaxHp); //���۽� �ִ� HP�� �ʱ�ȭ
        player_Mp = player_MaxMp;
        playerMp.SetPlayerMp(player_Mp, player_MaxMp); //���۽� �ִ� MP�� �ʱ�ȭ
    }

    private void OnTriggerEnter2D(Collider2D _collision) // � �ݶ��̴��� �������� �� �ߵ��Ǵ� Ʈ����(�Լ�)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object")) // ������ �ݶ��̴��� ���̾ interaction object(��ȣ�ۿ� ������Ʈ)�� ���
        {
            Debug.Log("��ȣ�ۿ� ������Ʈ ����");
            GameObject obj = _collision.gameObject; ;// obj�� ������ ������Ʈ
            interObj = obj.GetComponent<InteractionObject>(); // stratObject�� ������ ������Ʈ�� startobj�� ������.(�̷��� �� ������ startobj�� ���� ������Ʈ�� 2���ε� Ʈ���ŵ� ������Ʈ�� ���� ���� �׼��� ���ϱ����� ������ ��)

            eObjectType eOType = interObj.GetComponent<ObjectType>().GetObjectType(); // ������Ʈ Ÿ���� �ν����Ϳ��� ������ ������Ʈ Ÿ���� ����.
            switch (eOType) 
            {
                case eObjectType.Basic:
                    basicObj = true; break;
                case eObjectType.Enemy:
                    enemyObj = true; break;
            }
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Spawn") // ���� �������� �� ��� �� ���� Ʈ����
        {
            Debug.Log("���� Ʈ����");
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
            Debug.Log("ü�¿� ��ȭ�� ����");
        }
    }

    private void Moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal"); // ���η� �����̴� Ű �Է��� �޾ƿ�(�¿� ���ο�Ű, A, D)
        myRigid.velocity = new Vector2(moveDir.x * player_Speed, moveDir.y);// addforce�� ������ ���� ���������� ������ �ۿ��Ѵ�. velocity�� ������ �����ϰ� ���������� �ӵ��� ��ȭ�Ͽ� �ﰢ���� �������� �����ϴ�.

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
                if (hit.transform.tag == "Move Object") // �����̴� ���ǿ� �ö��� �� ���� �̵������ �ӵ��� ���� �÷��̾ ������
                {
                    routineF = movingTri.routineF;
                    myRigid.velocity = new Vector2(myRigid.velocity.x + routineF, verticalVelocity);
                }
            }
        }

    }

    private void CheckGravity()
    {
        if (isGround == false) // ���߿� ���� ���(isGround �� true > false �� ���ڸ���)
        {
            verticalVelocity -= gravity * Time.deltaTime; // verticalVelocity�� ������Ʈ�� y�� - �������� �޴� ���� ũ���̰� �߷°��� ���Ƿ� �����ϱ����� ��ũ��Ʈ�� �ۼ��߱⿡ �߷��� �޴� �� ó�� ���̰� �Ϸ��� ���߿� ���� ������ y�� - �������� ��� ���� �������� �Ѵ�.
            if (verticalVelocity < fallingLimit) // �������� ���� ������ limit������ �۾� �� ���(�������� ���� ������ ������ Ŀ�����) limit������ ����.
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            if (isJump == true)
            {
                isJump = false;
                verticalVelocity = jumpForce; // �����̽��ٸ� ������ �� y�� �������� ���� ����.
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

    private void InteractionObj() // ��ȣ�ۿ��� ������Ʈ�� ���� �ִ� ������ �� ��ų ȹ�� �Լ�
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
