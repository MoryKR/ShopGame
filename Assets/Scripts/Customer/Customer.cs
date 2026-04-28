using UnityEngine;
using UnityEngine.AI;

// 손님 행동 (NavMesh 기반 이동)
public class Customer : MonoBehaviour
{
    public enum CustomerState
    {
        Wander,
        MovingToCounter,
        WaitingAtCounter,
        Leaving
    }

    [SerializeField] private float arriveThreshold = 0.1f;
    [SerializeField] private int sampleMaxAttempts = 10;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private CustomerState state = CustomerState.Wander;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 2D NavMesh 설정
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        PickNewWanderTarget();
    }

    private void Update()
    {
        UpdateFlip();

        // 1단계는 Wander 상태만 동작
        if (state == CustomerState.Wander) UpdateWander();
    }

    // Wander 상태: 도착하면 새 목표 지점 선택
    private void UpdateWander()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance > arriveThreshold) return;
        PickNewWanderTarget();
    }

    // 현재 위치 기준 반경 내 NavMesh 위 랜덤 점을 목표로 설정
    private void PickNewWanderTarget()
    {
        float radius = Managers.Customer.WanderRadius;

        for (int i = 0; i < sampleMaxAttempts; i++)
        {
            Vector2 random2D = Random.insideUnitCircle * radius;
            Vector3 candidate = transform.position + new Vector3(random2D.x, random2D.y, 0f);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }
    }

    // 이동 방향에 따라 스프라이트 좌우 반전
    private void UpdateFlip()
    {
        if (spriteRenderer == null) return;
        float vx = agent.velocity.x;
        if (vx > 0.01f) spriteRenderer.flipX = false;
        else if (vx < -0.01f) spriteRenderer.flipX = true;
    }
}
