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

        instance.game.Init();
        instance.customer.Init();
    }
}
