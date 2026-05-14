using UnityEngine;

// Popup UI 베이스 클래스
public abstract class UI_Popup : UI_Base
{
    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopup(this);
    }
}