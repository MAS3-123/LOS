using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D myRigid;
    [SerializeField] private BoxCollider2D colliLeg;
    [SerializeField] protected Animator myAnimator;
    [SerializeField] private SpriteRenderer deadSprite;

    protected eSkillType mySkillType;

    protected Player playerSC;
    protected EnemyHp enemySC;

    private GameObject hpBarSpawnObj;
    private GameObject myHpBar;
    private GameObject player;

    protected bool trigger = false;
    protected bool isGround = false;
    protected bool isJump = false;

    protected Vector3 playerVec;
    protected Vector2 EPVec;
    private Vector3 enemyVec;

    protected float EPVecX = 0f;
    protected float EPVecY = 0f;
    private float time = 0f;
    private float invisiblTime = 0f;

    private float gravity = 30.0f;
    private float groundRatio = 0.02f;
    protected float jumpForce = 12.0f;
    protected float verticalVelocity = 0f;
    protected int fallingLimit = -15;

    protected int count = 0;
    public int enemyHp; // private로 바꾸기
    private int previous_enemyHp;
    public int enemy_MaxHp = 10; // private로 바꾸기

    public int p_enemeyHp
    {
        get { return enemyHp; }
        set
        {
            enemyHp += value;

            if (enemyHp != previous_enemyHp) // 체력 변화가 생기면
            {
                Debug.Log($"{gameObject.name}의 체력에 변화가 생김");
                if (enemyHp < previous_enemyHp)
                {
                    EnemyHit();
                }
                previous_enemyHp = enemyHp;
                if (enemySC != null)
                {
                    enemySC.SetEnemyHp(p_enemeyHp, enemy_MaxHp);
                }
            }
        }
    }

    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        myAnimator.SetBool("Hit", false);
    }

    public virtual void Awake()
    {

    }

    private void Start()
    {
        playerSC = GameManager.Instance.playerObj.GetComponent<Player>();
        player = playerSC.gameObject;

        hpBarSpawnObj = GameManager.Instance.enemyHpBarObj;
        p_enemeyHp = enemy_MaxHp;
        previous_enemyHp = p_enemeyHp;
        if (myHpBar == null)
        {
            GameObject obj = Resources.Load<GameObject>("Prefebs/EnemyHpBar");
            myHpBar = Instantiate(obj, gameObject.transform.position, Quaternion.identity, hpBarSpawnObj.transform);
            myHpBar.gameObject.name = $"{gameObject.name} HpBar";
            enemySC = myHpBar.GetComponent<EnemyHp>();
            enemySC.SetEnemyHp(p_enemeyHp, enemy_MaxHp);

            EnemyHpBarPos();
        }
    }

    void Update()
    {
        EPVector();
        PlayerVisible();

        CheckGround();
        CheckGravity();

        if (trigger == true)
        {
            EnemyMoving();
            SkillOn();

            EnemyDead();
        }

        if (enemySC != null)
        {
            EnemyHpBarPos();
        }
    }

    private void PlayerVisible()
    {
        if (Mathf.Abs(EPVecX) > 9f && trigger == true)
        {
            invisiblTime += Time.deltaTime;
            if (invisiblTime > 5)
            {
                Debug.Log("플레이어와의 거리가 멈");
                trigger = false;
            }
        }
        else if (Mathf.Abs(EPVecX) <= 9f && trigger != true)
        {
            OnBecameMainCamera();
        }
    }

    private void EPVector()
    {
        enemyVec = gameObject.transform.position;

        if (player != null)
        {
            playerVec = player.transform.position;
        }

        EPVecX = playerVec.x - enemyVec.x;
        EPVecY = playerVec.y - enemyVec.y;
    }

    private void OnBecameMainCamera()
    {
        Debug.Log($"trigger On objectName = {gameObject.name}");
        trigger = true;

        time = 0f;
        invisiblTime = 0f;
    }

    private void EnemyHit()
    {
        myAnimator.SetBool("Hit", true);
        StartCoroutine(HitAnimation());
    }

    private void EnemyHpBarPos()
    {
        Vector3 fixedPos = transform.position;
        fixedPos.z = 0;

        fixedPos = Camera.main.WorldToScreenPoint(fixedPos);
        fixedPos.y -= 50f;
        enemySC.transform.position = fixedPos;
    }

    private void EnemyDead()
    {
        if (p_enemeyHp <= 0)
        {
            GameObject obj = Instantiate(GameManager.Instance.interactionObj, transform.position + new Vector3(0, 1, 0),
                                         Quaternion.identity, GameManager.Instance.dynamicObj.transform); // 플레이어가 상호작용 할 수 있는 오브젝트 소환
            InteractionObject interObj = obj.GetComponent<InteractionObject>();
            SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
            PlayerSkillType(obj, interObj); // 넘겨줄 정보
            string skillName = obj.name.Substring(obj.name.IndexOf('_') + 1) + "_Skill"; // 플레이어가 얻게 될 스킬 프리팹 이름
            interObj.included_Skill[0] = Resources.Load<GameObject>($"Prefebs/GetEnemySkill/{skillName}");
            spr.sprite = deadSprite.sprite;

            Destroy(gameObject);
            Destroy(myHpBar);
        }
    }

    private void EnemyMoving()
    {
        if (EPVecX > 5)
        {
            EPVecX = 5;
        }
        else if (EPVecX < -5)
        {
            EPVecX = -5;
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

    public virtual void PlayerSkillType(GameObject _obj, InteractionObject _interObj)
    {

    }
}
