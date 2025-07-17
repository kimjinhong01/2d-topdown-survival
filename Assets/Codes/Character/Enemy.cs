using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon; // 애니메이터 컨트롤러
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait; // 다음 FixedUpdate 할때까지 기다림

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        // 게임이 살아있다면 실행

        // 적이 죽었거나 현재 애니메이터 상태가 Hit 이면 돌아가기
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec); // 해당 위치로 이동
        rigid.velocity = Vector2.zero; // 물리 속도가 이동에 영향을 주지 않도록
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        // 게임이 살아있다면 실행

        if (health <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
        }

        if (!isLive)
            return;
        // 적이 살아있다면 실행

        // 플레이어가 왼쪽에 있다면 true, 뒤집기
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        // 활성화되면 target을 player로 설정
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        // 애니메이터를 data 타입에 맞춰 설정
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!collision.CompareTag("Bullet") || !isLive) && !collision.CompareTag("HitBox"))
            return;

        if (collision.CompareTag("HitBox"))
        {
            health -= 4.5f;
            Debug.Log("근접공격 맞음!");
        }
        else
        {
            health -= collision.GetComponent<Bullet>().damage;
        }
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);

            if (GameManager.instance.isLive) // 끝나고 다 뒤졌을때 사운드 테러 안당하게
            {
                GameManager.instance.kill++;
                GameManager.instance.GetExp();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 1, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
