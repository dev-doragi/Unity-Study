using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float max_speed;
    public float jump_power;

    bool isJump = false;

    float rayLength = 2.2f;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Vector3 movevelocity = Vector3.zero;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Reverse();
        Jump();
    }

    private void FixedUpdate()
    {
        Debug.Log(isJump);
        // h에 "Horizontal" 즉 좌우 방향키로 1, -1을 받아옴
        float h = Input.GetAxisRaw("Horizontal");

        //변수에 x축으로 더해줄 속도를 입력해줌
        movevelocity = new Vector3(max_speed, 0, 0);
        //현재 나의 위치를 컴퓨터 사양에 맞는 프레임 단위로 계속 이동해준다.
        transform.position += movevelocity * h * Time.deltaTime;

        //만약 내 속도가 최고 속도를 넘는다면
        if (rigid.velocity.x > max_speed)
            //내 속도를 최고 속도로 고정한다.
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        else if (rigid.velocity.x < max_speed * (-1))
            rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);

        // 내가 움직이고 계속 움직이는 중이라면 변수를 0으로 바꾼다. == 내가 방향키에서 손을 땠지만 아직 속도가 있다면 그 속도를 0으로 바꾸겠다.
        if (rigid.velocity.x != 0)
        {
            Vector3 moveVelocity = Vector3.zero;
        }

        //레이케스트
        //아래쪽으로 레이져를 쏴서 아래에 "Platform"이라는 레이어의 물체가 있는지 판단
        Debug.DrawRay(rigid.position, Vector3.down * rayLength, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, rayLength, LayerMask.GetMask("Platform"));
        //만약 "platform"이라는 레이어의 물체가 있으면 점프를 하고 있지 않다는 의미 == 내가 땅에 붙어있다.
        if (rayHit.collider != null) // null을 부정하기 때문에 없지 않다. == 있다.
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

    void onDamaged(Vector2 targetPos)
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        Invoke("offDamaged", 3);
    }

    void offDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    //점프 구현
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            Debug.Log("점프!");
            isJump = true;
        }
    }


    //고개 돌리기
    void Reverse()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Horizontal"))
            if (h == 1)
                spriteRenderer.flipX = false;
            else if (h == -1)
                spriteRenderer.flipX = true;
    }
}
