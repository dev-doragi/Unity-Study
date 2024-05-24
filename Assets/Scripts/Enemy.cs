using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    public float maxSpeed;
    private bool movingjudge;
    float r = 0.0f;
    public int h = 0;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriterenderer;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        r = Random.Range(3.0f, 7.0f);
        Invoke("Onmoving", r);
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {

        Vector3 movevelocity = Vector3.zero;

        movevelocity = new Vector3(maxSpeed, 0, 0);
        transform.position += movevelocity * h * Time.deltaTime;

        //최대 속력
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        if (rigid.velocity.x != 0)
        {
            Vector3 moveVelocity = Vector3.zero;

        }

        Vector2 PlatformCheck = new Vector2(rigid.position.x + h, rigid.position.y);
        Debug.DrawRay(PlatformCheck, Vector3.down, new Color(0, 1, 0)); // 실제 Scene에 Raycast를 보여주는 디버그
        RaycastHit2D rayHit = Physics2D.Raycast(PlatformCheck, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            h *= -1;
            CancelInvoke(); // 실행되는 모든 Invoke를 취소하는 함수
            Invoke("Onmoving", 5);
        }

        if (h == 1)
            spriterenderer.flipX = false;
        else if (h == -1)
            spriterenderer.flipX = true;
    }

    void Onmoving()
    {
        h = Random.Range(-1, 2);
        if (h == 0)
        {
            anim.SetBool("Moving", false);
        }
        else if (h == 1)
        {
            spriterenderer.flipX = false;
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", true);
            spriterenderer.flipX = true;
        }
        r = Random.Range(3.0f, 7.0f);
        Invoke("Onmoving", r); // 재귀 호출
    }
}