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
        //AddForce�� ����� ������ ����
        float h = Input.GetAxisRaw("Horizontal");
        //h�� �������� Impulse ������ ���� �ְڴ�.
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //�ִ� �ӷ�
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
        //�¿� ����Ű���� ���� ���� �ӷ��� 0.5�� ���̰ڴ�.
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        // ���� ���� x���� �ӷ��� 0.5���� ������ �ӷ��� ������ 0���� �ְڴ�.
        if(Mathf.Abs(rigid.velocity.x) < 0.5f)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);

        }
    }
}