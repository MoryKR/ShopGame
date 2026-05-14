using UnityEngine;
using UnityEngine.UI;

// 밤 전환 Popup
public class UI_NightPopup : UI_Popup
{
    enum Buttons
    {
        StartDayButton
    }

    enum Texts
    {
        TitleText,
        MessageText
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.StartDayButton).onClick.AddListener(OnStartDayClicked);
        
        GetText((int)Texts.TitleText).text = "밤이 되었습니다";
        GetText((int)Texts.MessageText).text = "하루 매출을 정산하고\n다음 날을 준비하세요";
    }

    private void OnStartDayClicked()
    {
        Managers.Game.ChangeToDay();
        ClosePopup();
    }
}