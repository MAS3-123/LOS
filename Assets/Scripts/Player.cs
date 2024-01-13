using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] SpriteRenderer spr;
    [Space]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] PolygonCollider2D colli;
    [SerializeField] BoxCollider2D colliLeg;

    Vector2 moveDir = Vector2.zero;

    public GameObject[] skill;

    public bool isJump = false;
    public bool isGround = false;
    public bool basicObj = false;
    public bool enemyObj = false;
    [Space]
    public float gravity = 30.0f;
    public float jumpForce = 20.0f;
    public float groundRatio = 0.02f;
    private float verticalVelocity = 0f;
    
    public int moveSpeed = 5;
    public int fallingLimit = 10;

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object") && collision.gameObject.tag == "Basic Object") // 처음 상호작용 하게 될 오브젝트
        {
            basicObj = true;
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Interaction Object") && collision.gameObject.tag == "Enemy Object") // 적 처치시 상호작용 하게 될 오브젝트
        {
            enemyObj = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        basicObj = false;
        enemyObj = false;
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
        rigid.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y);

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
            }
        }

    }

    private void CheckGravity()
    {
        if(isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if(verticalVelocity > fallingLimit)
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

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void InteractionObj() // 상호작용한 오브젝트가 갖고 있는 아이템 및 스킬 획득 함수
    {
        if(basicObj == true)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                skill[0] = StartObj.Instance.included_Skill[0];
                Debug.Log("basic");
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
}
