using UnityEngine;
using UnityEngine.UI;

// 손님 머리 위 버블 UI 관리
public class CustomerBubbleUI : MonoBehaviour
{
    private Canvas canvas;
    private Button button;
    private Customer customer;

    // 초기화 및 UI 생성
    public void Init(Customer owner)
    {
        customer = owner;
        CreateUI();
        Hide();
    }

    private void CreateUI()
    {
        // Canvas 생성
        GameObject canvasObj = new GameObject("BubbleCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = new Vector3(0f, 2f, 0f);

        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;

        GraphicRaycaster raycaster = canvasObj.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(100f, 40f);
        canvasRect.localScale = new Vector3(0.06f, 0.08f, 0.06f);

        // Button 생성
        GameObject buttonObj = new GameObject("BubbleButton");
        buttonObj.transform.SetParent(canvasObj.transform);
        buttonObj.transform.localPosition = Vector3.zero;
        buttonObj.transform.localScale = Vector3.one;

        button = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.sizeDelta = new Vector2(100f, 40f);
        buttonRect.anchoredPosition = Vector2.zero;

        // BubbleUI 스프라이트 로드
        Sprite bubbleSprite = Resources.Load<Sprite>("UI/BubbleUI");
        if (bubbleSprite != null)
            buttonImage.sprite = bubbleSprite;
        else
            Debug.LogWarning("CustomerBubbleUI: UI/BubbleUI 스프라이트를 찾을 수 없습니다.");

        // 버튼 클릭 이벤트 연결
        button.onClick.AddListener(OnBubbleClicked);
    }

    private void Update()
    {
        // 카메라를 향하도록 회전 (billboard)
        if (canvas != null && Camera.main != null)
        {
            canvas.transform.LookAt(canvas.transform.position + Camera.main.transform.forward);
        }
    }

    public void Show()
    {
        if (canvas != null) canvas.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (canvas != null) canvas.gameObject.SetActive(false);
    }

    private void OnBubbleClicked()
    {
        if (customer != null)
            customer.SetState(Customer.CustomerState.Leaving);
    }
}