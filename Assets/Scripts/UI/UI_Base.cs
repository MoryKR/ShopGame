using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 컴포넌트 바인딩 베이스 클래스
public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T>(Type enumType) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(enumType);
        UnityEngine.Object[] arr = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), arr);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                arr[i] = Util.FindChild(gameObject, names[i], true);
            else
                arr[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (arr[i] == null)
                Debug.LogWarning($"UI_Base: {names[i]} 바인딩 실패");
        }
    }

    protected T GetUI<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] arr = null;
        if (objects.TryGetValue(typeof(T), out arr) == false)
            return null;

        return arr[idx] as T;
    }

    protected GameObject GetObject(int idx) { return GetUI<GameObject>(idx); }
    protected Text GetText(int idx) { return GetUI<Text>(idx); }
    protected Button GetButton(int idx) { return GetUI<Button>(idx); }
    protected Image GetImage(int idx) { return GetUI<Image>(idx); }

    public abstract void Init();
}

// UI 탐색 유틸리티
public static class Util
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) return null;
        return transform.gameObject;
    }
}