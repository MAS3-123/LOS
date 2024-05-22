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

    [Header("����")]
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
            if(value < 0) // ������ �Ծ��� ��
            {
                if (immunity == true) 
                {
                    value = 0;
                }
                else
                {
                    immunity = true;
                    StartCoroutine(ImmunityDamage());
                    Debug.Log("�鿪 Ȱ��ȭ");
                    
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

    [Header("HP ����")]
    [SerializeField] private PlayerHp playerHp_SC;

    [Space]
    [Header("MP ����")]
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
    { // �˹� ���� �� ���� �Լ�
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
            Debug.Log("���� ���� ȸ����");
            if (playerMp == playerMaxMp)
            {
                manaRecovering = false;
                Debug.Log("���� ���� ȸ�� ��");
                break;
            }
        }
    }

    IEnumerator ImmunityDamage()
    {
        yield return new WaitForSeconds(2f);
        immunity = false;
        Debug.Log("�鿪 ��Ȱ��ȭ");
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
        playerHp_SC.SetPlayerHp(playerHp, playerMaxHp); //���۽� �ִ� HP�� �ʱ�ȭ
        playerMp = playerMaxMp;
        playerMp_SC.SetPlayerMp(playerMp, playerMaxMp); //���۽� �ִ� MP�� �ʱ�ȭ
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

    private void OnTriggerEnter2D(Collider2D _collision) // � �ݶ��̴��� �������� �� �ߵ��Ǵ� Ʈ����(�Լ�)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object")) // ������ �ݶ��̴��� ���̾ interaction object(��ȣ�ۿ� ������Ʈ)�� ���
        {
            Debug.Log("��ȣ�ۿ� ������Ʈ ����");
            GameObject obj = _collision.gameObject; ;// obj�� ������ ������Ʈ
            interObj = obj.GetComponent<InteractionObject>(); // stratObject�� ������ ������Ʈ�� startobj�� ������.(�̷��� �� ������ startobj�� ���� ������Ʈ�� 2���ε� Ʈ���ŵ� ������Ʈ�� ���� ���� �׼��� ���ϱ����� ������ ��)

            isInterObj = true;

            interObj.myAnimator.SetBool("Interection Player", true);
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Trigger") && _collision.gameObject.tag == "Spawn") // ���� �������� �� ��� �� ���� Ʈ����
        {
            Debug.Log("���� Ʈ����");
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
        moveDir.x = Input.GetAxisRaw("Horizontal"); // ���η� �����̴� Ű �Է��� �޾ƿ�(�¿� ���ο�Ű, A, D)
        myRigid.velocity = new Vector2(moveDir.x * playerSpeed, moveDir.y);// addforce�� ������ ���� ���������� ������ �ۿ��Ѵ�. velocity�� ������ �����ϰ� ���������� �ӵ��� ��ȭ�Ͽ� �ﰢ���� �������� �����ϴ�.

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
                if (hit.transform.tag == "Move Object_Horizotal") // �����̴� ���ǿ� �ö��� �� ���� �̵������ �ӵ��� ���� �÷��̾ ������
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
                damageOn = false;
                myAnimator.SetBool("Jump", false);
                myAnimator.SetBool("StandByJump", true);
            }
        }
        myRigid.velocity = new Vector2(myRigid.velocity.x, verticalVelocity);
    }

    private void InteractionObj() // ��ȣ�ۿ��� ������Ʈ�� ���� �ִ� ������ �� ��ų ȹ�� �Լ�
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
            transform.GetChild(0).SetParent(default); // ī�޶� ������ ��
            Destroy(gameObject);
        }
    }

    public void PlayerKnockBack()
    {
        if (damageOn == true)
        {
            if (EPVecX > 0 && EPVecX < 0.5) // �� ����
            {
                EPVecX = 0.5f;
            }
            else if (EPVecX < 0 && EPVecX > -0.5) // �� ����
            {
                EPVecX = -0.5f;
            }

            Debug.Log("������ �������� �Ծ� �˹� ��");
            verticalVelocity = (4f / Mathf.Abs(EPVecX)); //EPVecX�� ���� �� �Ÿ� ����
            myRigid.velocity = Vector3.zero;
            myRigid.AddForce(new Vector2((1 / EPVecX) * 2f, 0), ForceMode2D.Impulse);
        }
    }
}
