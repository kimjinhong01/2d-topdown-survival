using UnityEngine;

public class RepositionEnemy : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area Enemy"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // 적 위치

        switch (transform.tag)
        {
            case "Enemy":
                if (coll.enabled) // collider가 활성화되어 있다면
                {
                    /* 임시 방편
                    Vector3 dist = playerPos - myPos; // 플레이어와의 거리
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0); // 랜덤 위치
                    transform.Translate(ran + dist * 2); // 랜덤 위치 + 거리 x 2 만큼 이동시킨다
                    */

                    this.GetComponent<Enemy>().health = 0;
                }
                break;
        }
    }
}
