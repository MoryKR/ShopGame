using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 관리 매니저
public class UIManager
{
    private int order = 10;
    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    private Canvas rootCanvas;

    public void Init()
    {
        GameObject canvasObj = GameObject.Find("@UI_Root");
        if (canvasObj == null)
        {
            canvasObj = new GameObject { name = "@UI_Root" };
            canvasObj.AddComponent<Canvas>();
            canvasObj.AddComponent<GraphicRaycaster>();
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            rootCanvas = canvasObj.GetComponent<Canvas>();
            rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            Object.DontDestroyOnLoad(canvasObj);
        }
        else
        {
            rootCanvas = canvasObj.GetComponent<Canvas>();
        }
    }

    public T ShowPopup<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/{name}");
        if (prefab == null)
        {
            Debug.LogWarning($"UIManager: {name} 프리팹을 찾을 수 없습니다.");
            return null;
        }

        GameObject go = Object.Instantiate(prefab);
        T popup = go.GetOrAddComponent<T>();
        popupStack.Push(popup);

        go.transform.SetParent(rootCanvas.transform);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = order++;

        popup.Init();

        return popup;
    }

    public void ClosePopup(UI_Popup popup)
    {
        if (popupStack.Count == 0) return;

        if (popupStack.Peek() != popup)
        {
            Debug.LogWarning("UIManager: 최상단 Popup이 아닙니다.");
            return;
        }

        ClosePopup();
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Object.Destroy(popup.gameObject);
        order--;
    }

    public void CloseAllPopups()
    {
        while (popupStack.Count > 0)
            ClosePopup();
    }
}

// GameObject Extension
public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }
}