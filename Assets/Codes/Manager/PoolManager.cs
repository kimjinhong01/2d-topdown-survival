using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        // 프리팹이 두 개면 풀도 두 개
        pools = new List<GameObject>[prefabs.Length];

        // 리스트에 있는 모든 데이터 초기화
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // 게임 오브젝트를 반환하는 함수
    public GameObject Get(int index)
    {
        // 놀고있는 오브젝트 하나를 선택하는 변수
        GameObject select = null;

        // 선택한 풀의 놀고 (비활성화 된) 있는 게임오브젝트 접근
        foreach (GameObject item in pools[index]) // 배열, 리스트들의 데이터를 순차적으로 접근하는 반복문
        {
            // 내용물 오브젝트가 비황성화(대기 상태)인지 확인
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform); // 원본 오브젝트를 복제하여 생성하는 함수
            pools[index].Add(select); // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 Add 함수로 추가
        }

        return select;
    }
}
