using UnityEngine;

// 게임 진행 관리
public class GameManager
{
    public enum DayNightState
    {
        Day,
        Night
    }

    private float dayDuration = 60f;
    private DayNightState currentState = DayNightState.Day;
    private float dayTimer = 0f;

    public DayNightState CurrentState { get { return currentState; } }

    public void Init()
    {
        currentState = DayNightState.Day;
        dayTimer = 0f;
        Debug.Log("GameManager: 낮 시작");
    }

    public void Update()
    {
        // Day 상태에서만 타이머 증가
        if (currentState == DayNightState.Day)
        {
            dayTimer += Time.deltaTime;

            if (dayTimer >= dayDuration)
            {
                ChangeToNight();
            }
        }

        // Night 상태에서 스페이스바 입력 시 낮으로 전환
        if (currentState == DayNightState.Night && Input.GetKeyDown(KeyCode.Space))
        {
            ChangeToDay();
        }
    }

    private void ChangeToNight()
    {
        currentState = DayNightState.Night;
        
        // 모든 손님 제거
        Managers.Customer.RemoveAllCustomers();
        
        Debug.Log("GameManager: 밤으로 전환 (추후 Popup 표시 예정)");
    }

    private void ChangeToDay()
    {
        currentState = DayNightState.Day;
        dayTimer = 0f;
        Debug.Log("GameManager: 낮으로 전환");
    }
}