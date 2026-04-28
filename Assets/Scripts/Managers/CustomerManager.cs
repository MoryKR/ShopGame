using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 손님 스폰 및 관리
public class CustomerManager
{
    private int maxCustomerCount = 3;
    private float spawnInterval = 10f;
    private float wanderRadius = 10f;

    private GameObject customerPrefab;
    private Transform spawnPoint;
    private List<Customer> customers = new List<Customer>();

    private MonoBehaviour coroutineRunner;

    public int MaxCustomerCount { get { return maxCustomerCount; } }
    public float WanderRadius { get { return wanderRadius; } }
    public IReadOnlyList<Customer> Customers { get { return customers; } }

    // 매니저 초기화 및 스폰 코루틴 시작
    public void Init()
    {
        coroutineRunner = Managers.Instance;

        // Resources/Prefab 폴더에서 Customer 프리팹 로드
        customerPrefab = Resources.Load<GameObject>("Prefab/Customer");

        // 씬에서 CustomerSpawnPoint 탐색
        GameObject spawnObj = GameObject.Find("CustomerSpawnPoint");
        if (spawnObj != null) spawnPoint = spawnObj.transform;

        coroutineRunner.StartCoroutine(SpawnLoop());
    }

    // 일정 시간마다 최대치 미만이면 손님 스폰
    private IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);
        while (true)
        {
            yield return wait;
            if (customers.Count < maxCustomerCount) SpawnCustomer();
        }
    }

    // 손님 인스턴스 생성 후 리스트에 추가
    private void SpawnCustomer()
    {
        if (customerPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("CustomerManager: customerPrefab 또는 spawnPoint가 비어 있습니다.");
            return;
        }

        GameObject go = Object.Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Customer customer = go.GetComponent<Customer>();
        if (customer != null) customers.Add(customer);
    }

    // 손님 제거 (Customer 측에서 호출)
    public void RemoveCustomer(Customer customer)
    {
        if (customer == null) return;
        customers.Remove(customer);
        Object.Destroy(customer.gameObject);
    }
}
