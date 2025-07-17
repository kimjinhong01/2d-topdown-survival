using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    void Awake()
    {
        // 자신을 포함한 모든 자식의 Transform 컴포넌트를 가져옴
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        // 120초 / 6개 몬스터라면 -> 20초 마다 level이 올라간다
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        // 최소값(인덱스 에러 제거용), 소수점 버리고 Int로 변환(게임 시간 / 몬스터 바뀌는 시간)
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // enemy가 0번째에 있기 때문에 Get(0)
        GameObject enemy = GameManager.instance.pool.Get(0);
        // 스폰포인트 중에 한 곳에서 랜덤으로 스폰 (0은 자기 자신이라 1부터)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy에 현재 레벨에 맞는 spawnData 제공
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// spawnData 클래스 생성, 밖에서 편집 가능하게
[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}