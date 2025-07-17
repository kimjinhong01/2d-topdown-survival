using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public float attackSpeed;
    float count = 0;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    public GameObject hitBoxL;
    public GameObject hitBoxR;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // 키보드 입력받아서 Vector2값에 넣기
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        count += Time.fixedDeltaTime;
        if (count > attackSpeed)
        {
            count = 0;
            anim.SetTrigger("Attack");
            if (spriter.flipX)
            {
                hitBoxL.SetActive(true);
                Invoke("HitBoxOff", 1f);
            }
            else
            {
                hitBoxR.SetActive(true);
                Invoke("HitBoxOff", 1f);
            }
        }

        // 모든 방향의 속도를 같게 normalized X 속도
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // Speed 파라미터값을 vector 자체의 크기값으로 설정
        anim.SetFloat("Speed", inputVec.magnitude);

        // 좌우 방향키 눌렀을때
        if (inputVec.x != 0)
        {
            // 좌측 방향키면 true
            spriter.flipX = inputVec.x < 0;
        }
    }

    // Collider랑 충돌 중일때 실행
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;
        // 살아있다면 아래 실행

        anim.SetTrigger("Hit");
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    void HitBoxOff()
    {
        hitBoxL.SetActive(false);
        hitBoxR.SetActive(false);
    }
}
