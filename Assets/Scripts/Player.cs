using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem effect;
    public float max_speed;
    public float jump_power;

    bool isJump = false;
    bool canMove = true;

    float rayLength = 3.2f;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Vector3 movevelocity = Vector3.zero;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vdown();
        Reverse();
        Jump();
        Move();
    }

    private void FixedUpdate()
    {
        // 레이케스트
        // 아래쪽으로 레이저를 쏴서 아래에 "Platform"이라는 레이어의 물체가 있는지 판단
        Debug.DrawRay(rigid.position, Vector3.down * rayLength, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, rayLength, LayerMask.GetMask("Platform"));
        // 만약 "platform"이라는 레이어의 물체가 있으면 점프를 하고 있지 않다는 의미 == 내가 땅에 붙어있다.
        if (rayHit.collider != null)
        {
            isJump = false;
        }
        else
        {
            isJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            onDamaged(collision.transform.position);
    }

    void constraintMove()
    {
        canMove = !canMove;
    }

    void onDamaged(Vector2 targetPos)
    {
        anim.SetTrigger("isHit");
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        Invoke("constraintMove", 0.4f);
        Invoke("constraintMove", 0.7f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(dirc * 8f, 7f), ForceMode2D.Impulse);
        isJump = false;

        Invoke("offDamaged", 1f);
    }

    void offDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    void Move()
    {
        if (canMove)
        {
            // h에 "Horizontal" 즉 좌우 방향키로 1, -1을 받아옴
            float h = Input.GetAxisRaw("Horizontal");

            // 힘을 가할 방향과 크기를 설정
            Vector3 force = new Vector3(h * max_speed, 0, 0);

            // Rigidbody에 힘을 가해 이동시킴
            rigid.AddForce(force);

            // 만약 내 속도가 최고 속도를 넘는다면
            if (rigid.velocity.x > max_speed)
            {
                // 내 속도를 최고 속도로 고정한다.
                rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < max_speed * (-1))
            {
                // 내 속도를 최고 속도로 고정한다.
                rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);
            }

            // 애니메이션 설정을 이동 처리 내에서 업데이트
            if (Mathf.Abs(rigid.velocity.x) > 0.1f)
                anim.SetBool("isWalking", true);
            else
                anim.SetBool("isWalking", false);
        }
    }

    void Vdown()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
        }
    }

    // 점프 구현
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            Debug.Log("점프!");
            isJump = true;
        }
    }

    // 고개 돌리기
    void Reverse()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Horizontal"))
        {
            if (h == 1)
                spriteRenderer.flipX = false;
            else if (h == -1)
                spriteRenderer.flipX = true;
        }
    }
}
