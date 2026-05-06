using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 손님 스폰 및 관리
public class CustomerManager
{
    private int maxCustomerCount = 3;
    private float spawnInterval = 10f;
    private float wanderRadius = 5f;
    private float counterInterval = 3f;

    private GameObject[] customerPrefabs;
    private Transform spawnPoint;
    private Transform counterPoint;
    private Transform exitPoint;
    private List<Customer> customers = new List<Customer>();
    private Customer customerAtCounter = null;

    private MonoBehaviour coroutineRunner;

    public int MaxCustomerCount { get { return maxCustomerCount; } }
    public float WanderRadius { get { return wanderRadius; } }
    public Transform CounterPoint { get { return counterPoint; } }
    public Transform ExitPoint { get { return exitPoint; } }
    public IReadOnlyList<Customer> Customers { get { return customers; } }

    // 매니저 초기화 및 코루틴 시작
    public void Init()
    {
        coroutineRunner = Managers.Instance;
        customerPrefabs = Resources.LoadAll<GameObject>("Prefabs/Customer");

        GameObject spawnObj = GameObject.Find("CustomerSpawnPoint");
        if (spawnObj != null) spawnPoint = spawnObj.transform;

        GameObject counterObj = GameObject.Find("CounterPoint");
        if (counterObj != null) counterPoint = counterObj.transform;

        GameObject exitObj = GameObject.Find("ExitPoint");
        if (exitObj != null) exitPoint = exitObj.transform;

        coroutineRunner.StartCoroutine(SpawnLoop());
        coroutineRunner.StartCoroutine(CounterLoop());
    }

    // 일정 시간마다 최대치 미만이면 손님 스폰
    private IEnumerator SpawnLoop()
    {
        WaitForSeconds spawnWait = new WaitForSeconds(spawnInterval);
        bool loop = true;
        while (loop)
        {
            yield return spawnWait;
            if (customers.Count < maxCustomerCount) SpawnCustomer();
        }
    }

    // 일정 시간마다 카운터가 비어있으면 배회 중인 손님 1명을 카운터로 보냄
    private IEnumerator CounterLoop()
    {
        WaitForSeconds counterWait = new WaitForSeconds(counterInterval);
        bool loop = true;
        while (loop)
        {
            yield return counterWait;
            if (customerAtCounter == null) SendCustomerToCounter();
        }
    }

    // 살아있는 손님과 다른 종류를 랜덤 선택해 NavMesh 위에 스폰
    private void SpawnCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogWarning("CustomerManager: customerPrefabs 또는 spawnPoint가 비어 있습니다.");
            return;
        }

        HashSet<string> activeNames = new HashSet<string>();
        foreach (Customer c in customers)
        {
            if (c != null) activeNames.Add(c.gameObject.name.Replace("(Clone)", "").Trim());
        }

        List<GameObject> candidates = new List<GameObject>();
        foreach (GameObject p in customerPrefabs)
        {
            if (!activeNames.Contains(p.name)) candidates.Add(p);
        }

        if (candidates.Count == 0) return;

        Vector3 spawnPos = spawnPoint.position;
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPos, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            spawnPos = hit.position;
        else
        {
            Debug.LogWarning("CustomerManager: SpawnPoint 주변에 NavMesh가 없습니다.");
            return;
        }

        GameObject prefab = candidates[Random.Range(0, candidates.Count)];
        GameObject go = Object.Instantiate(prefab, spawnPos, Quaternion.identity);
        Customer customer = go.GetComponent<Customer>();
        if (customer != null) customers.Add(customer);
    }

    // 배회 중인 손님 중 랜덤 1명을 카운터로 보냄
    private void SendCustomerToCounter()
    {
        if (counterPoint == null)
        {
            Debug.LogWarning("CustomerManager: CounterPoint가 없습니다.");
            return;
        }

        List<Customer> wanderers = new List<Customer>();
        foreach (Customer c in customers)
        {
            if (c != null && c.State == Customer.CustomerState.Wander) wanderers.Add(c);
        }

        if (wanderers.Count == 0) return;

        Customer chosen = wanderers[Random.Range(0, wanderers.Count)];
        customerAtCounter = chosen;
        chosen.SetState(Customer.CustomerState.MovingToCounter);
    }

    // 카운터 점유 해제 (손님이 카운터를 떠날 때 호출)
    public void ClearCounter()
    {
        customerAtCounter = null;
    }

    // 손님 제거 (Leaving 완료 시 호출)
    public void RemoveCustomer(Customer customer)
    {
        if (customer == null) return;
        if (customerAtCounter == customer) customerAtCounter = null;
        customers.Remove(customer);
        Object.Destroy(customer.gameObject);
    }
}