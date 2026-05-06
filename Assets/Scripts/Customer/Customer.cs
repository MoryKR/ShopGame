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

    [SerializeField] private float arriveThreshold = 0.3f;
    [SerializeField] private int sampleMaxAttempts = 10;
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private float frameInterval = 0.15f;
    [SerializeField] private float moveThreshold = 0.05f;
    [SerializeField] private float wanderInterval = 3f;

    private NavMeshAgent agent;
    private SpriteRenderer spriteRenderer;
    private CustomerState state = CustomerState.Wander;
    private CustomerBubbleUI bubbleUI;

    private int currentFrame = 0;
    private float frameTimer = 0f;
    private float wanderTimer = 0f;

    public CustomerState State { get { return state; } }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 2D NavMesh 설정
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // BubbleUI 초기화
        bubbleUI = gameObject.AddComponent<CustomerBubbleUI>();
        bubbleUI.Init(this);
    }

    private void Start()
    {
        PickNewWanderTarget();
    }

    private void Update()
    {
        UpdateFlip();
        UpdateWalkAnimation();

        switch (state)
        {
            case CustomerState.Wander:           UpdateWander(); break;
            case CustomerState.MovingToCounter:  UpdateMovingToCounter(); break;
            case CustomerState.WaitingAtCounter: UpdateWaitingAtCounter(); break;
            case CustomerState.Leaving:          UpdateLeaving(); break;
        }
    }

    // 외부에서 상태 전환 시 호출
    public void SetState(CustomerState newState)
    {
        state = newState;

        switch (state)
        {
            case CustomerState.MovingToCounter:
                if (Managers.Customer.CounterPoint != null)
                    agent.SetDestination(Managers.Customer.CounterPoint.position);
                break;
            case CustomerState.WaitingAtCounter:
                agent.ResetPath();
                bubbleUI.Show();
                break;
            case CustomerState.Leaving:
                bubbleUI.Hide();
                if (Managers.Customer.ExitPoint != null)
                    agent.SetDestination(Managers.Customer.ExitPoint.position);
                break;
        }
    }

    // 정지 상태에서 wanderInterval 만큼 대기 후 새 목표 지점 선택
    private void UpdateWander()
    {
        if (agent.pathPending)
        {
            wanderTimer = 0f;
            return;
        }

        if (agent.velocity.magnitude > moveThreshold)
        {
            wanderTimer = 0f;
            return;
        }

        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            wanderTimer = 0f;
            PickNewWanderTarget();
        }
    }

    // CounterPoint 도착 시 WaitingAtCounter로 전환
    private void UpdateMovingToCounter()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance > arriveThreshold) return;
        SetState(CustomerState.WaitingAtCounter);
    }

    // 버튼 클릭 대기 (스페이스바 제거)
    private void UpdateWaitingAtCounter()
    {
        // 버튼 클릭으로 Leaving 전환 (CustomerBubbleUI.OnBubbleClicked에서 처리)
    }

    // ExitPoint 도착 시 제거 요청
    private void UpdateLeaving()
    {
        if (agent.pathPending) return;
        if (agent.remainingDistance > arriveThreshold) return;
        Managers.Customer.RemoveCustomer(this);
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
            if (NavMesh.SamplePosition(candidate, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                return;
            }
        }

        Debug.LogWarning(gameObject.name + ": PickNewWanderTarget 실패 - 주변에 NavMesh 없음");
    }

    // 이동 방향에 따라 스프라이트 좌우 반전
    private void UpdateFlip()
    {
        if (spriteRenderer == null) return;
        float vx = agent.velocity.x;
        if (vx > 0.01f) spriteRenderer.flipX = false;
        else if (vx < -0.01f) spriteRenderer.flipX = true;
    }

    // 이동 중일 때 일정 간격으로 스프라이트 교체, 정지 시 첫 프레임 고정
    private void UpdateWalkAnimation()
    {
        if (spriteRenderer == null) return;
        if (walkSprites == null || walkSprites.Length == 0) return;

        bool isMoving = agent.velocity.magnitude > moveThreshold;

        if (!isMoving)
        {
            currentFrame = 0;
            frameTimer = 0f;
            spriteRenderer.sprite = walkSprites[0];
            return;
        }

        frameTimer += Time.deltaTime;
        if (frameTimer >= frameInterval)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            spriteRenderer.sprite = walkSprites[currentFrame];
        }
    }
}