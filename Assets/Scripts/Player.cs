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
        // h�� "Horizontal" �� �¿� ����Ű�� 1, -1�� �޾ƿ�
        float h = Input.GetAxisRaw("Horizontal");

        //������ x������ ������ �ӵ��� �Է�����
        movevelocity = new Vector3(max_speed, 0, 0);
        //���� ���� ��ġ�� ��ǻ�� ��翡 �´� ������ ������ ��� �̵����ش�.
        transform.position += movevelocity * h * Time.deltaTime;

        //���� �� �ӵ��� �ְ� �ӵ��� �Ѵ´ٸ�
        if (rigid.velocity.x > max_speed)
            //�� �ӵ��� �ְ� �ӵ��� �����Ѵ�.
            rigid.velocity = new Vector2(max_speed, rigid.velocity.y);
        else if (rigid.velocity.x < max_speed * (-1))
            rigid.velocity = new Vector2(max_speed * (-1), rigid.velocity.y);

        // ���� �����̰� ��� �����̴� ���̶�� ������ 0���� �ٲ۴�. == ���� ����Ű���� ���� ������ ���� �ӵ��� �ִٸ� �� �ӵ��� 0���� �ٲٰڴ�.
        if (rigid.velocity.x != 0)
        {
            Vector3 moveVelocity = Vector3.zero;
        }

        //�����ɽ�Ʈ
        //�Ʒ������� �������� ���� �Ʒ��� "Platform"�̶�� ���̾��� ��ü�� �ִ��� �Ǵ�
        Debug.DrawRay(rigid.position, Vector3.down * rayLength, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, rayLength, LayerMask.GetMask("Platform"));
        //���� "platform"�̶�� ���̾��� ��ü�� ������ ������ �ϰ� ���� �ʴٴ� �ǹ� == ���� ���� �پ��ִ�.
        if (rayHit.collider != null) // null�� �����ϱ� ������ ���� �ʴ�. == �ִ�.
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

    //���� ����
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            Debug.Log("����!");
            isJump = true;
        }
    }


    //���� ������
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