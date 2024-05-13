using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ver2 : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //AddForce를 사용한 움직임 구현
        float h = Input.GetAxisRaw("Horizontal");
        //h의 방향으로 Impulse 형태의 힘을 주겠다.
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //최대 속력
        if (rigid.velocity.x > maxSpeed)//Right
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed*(-1)) //Left
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        }
    }

    private void Update()
    {
        //좌우 방향키에서 손을 때면 속력을 0.5씩 줄이겠다.
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        // 만약 현재 x축의 속력이 0.5보다 낮으면 속력을 완전히 0으로 주겠다.
        if(Mathf.Abs(rigid.velocity.x) < 0.5f)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);

        }
    }
}
