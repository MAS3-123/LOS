using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
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
    
    private float gravity = 30.0f;
    private float groundRatio = 0.02f;
    protected float jumpForce = 12.0f;
    protected float verticalVelocity = 0f;
    protected int fallingLimit = -15;

    protected int count = 0;
    public int enemyHp; // private로 바꾸기
    private int previous_enemyHp;
    private int enemy_MaxHp = 10;

    public int p_enemeyHp
    {
        get { return enemyHp; }
        set
        {
            enemyHp += value;

            if(enemyHp != previous_enemyHp) // 체력 변화가 생기면
            {
                Debug.Log("체력 변화가 생김");
                if(enemyHp < previous_enemyHp)
                {
                    EnemyHit();
                }
                previous_enemyHp = enemyHp;
                if(enemySC != null)
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

    private void OnBecameVisible()//이벤트함수
    {
        trigger = true;
        Debug.Log($"trigger On objectName = {gameObject.name}");

        playerSC = GameManager.Instance.playerObj.GetComponent<Player>();
        player = playerSC.gameObject;

        time = 0f;
        count++;

    }

    private void OnBecameInvisible()
    {
        trigger = false;
    }
    private void Start()
    {
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

        EnemyDead();

        if (enemySC != null)
        {
            EnemyHpBarPos();
        }
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
        if(p_enemeyHp <= 0)
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
        EPVecX = playerVec.x - enemyVec.x;
        EPVecY = playerVec.y - enemyVec.y;
        if (EPVecX > 5)
        {
            EPVecX = 5;
        }
        else if (EPVecX < -5)
        {
            EPVecX = -5;
        }

        if (time > 2f)
        {
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

    public virtual void PlayerSkillType(GameObject _obj, InteractionObject _interObj)
    {

    }
    // 첫 조우 후 플레이어가 멀어지면 화면 내 범위까지 접근 하지만 특정 시간 내에 접근하지 못하면 이동 정지
    // 조우시 갖고있는 스킬 쿨타임마다 사용
    // 체력이 0 됐을 때 그 위치에 상호작용 가능한 오브젝트 생성
    // 생성된 오브젝트는 호출한 Enemy의 스킬 및 특성 보유
}
