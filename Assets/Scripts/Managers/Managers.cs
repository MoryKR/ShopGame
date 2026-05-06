using UnityEngine;

// 게임 전체 매니저들을 통합 관리하는 싱글톤
public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance { get { Init(); return instance; } }

    private GameManager game = new GameManager();
    private CustomerManager customer = new CustomerManager();

    public static GameManager Game { get { return Instance.game; } }
    public static CustomerManager Customer { get { return Instance.customer; } }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        game.Update();
    }

    // 싱글톤 초기화 및 하위 매니저 Init 호출
    private static void Init()
    {
        if (instance != null) return;

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        instance = go.GetComponent<Managers>();

        // EventSystem 생성 (없으면 UI 버튼 클릭 불가)
        if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            DontDestroyOnLoad(eventSystemObj);
        }

        instance.game.Init();
        instance.customer.Init();
    }
}