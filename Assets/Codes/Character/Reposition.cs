using UnityEngine;

public class Reposition : MonoBehaviour
{
    // Collider 나가면 실행
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) // CompareTag : 태그 비교
            return;
        // Area가 벗어났다면 아래 명령어 실행

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position; // 타일맵 위치
        
        switch (transform.tag)
        {
            case "Ground": // Area가 타일맵을 벗어났다면
                float diffX = playerPos.x - myPos.x; // 좌우 거리
                float diffY = playerPos.y - myPos.y; // 상하 거리
                float dirX = diffX < 0 ? -1 : 1; // 좌우 방향
                float dirY = diffY < 0 ? -1 : 1; // 상하 방향
                diffX = Mathf.Abs(diffX); // Abs : 절댓값
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY) // 좌우 거리가 상하 거리보다 멀면
                {
                    // 타일을 좌우로 옮긴다, Translate : 해당 벡터만큼 이동
                    transform.Translate(Vector3.right * dirX * 80);
                }
                if (diffY > diffX) // 상하 거리가 좌우 거리보다 멀면
                {
                    // 타일을 상하로 옮긴다
                    transform.Translate(Vector3.up * dirY * 80);
                }
                break;
        }
    }
}
