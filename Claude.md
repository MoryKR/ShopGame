# ShopGame
ShopGame
# 프로젝트 개발 규칙 (CLAUDE.md)

---

## 코드 스타일 규칙
- 코드 구조는 단순하고 명확하게 작성한다
- 불필요한 추상화는 하지 않는다
- 함수 이름은 직관적으로 작성한다
- 주석은 한 줄로 간결하게 작성한다
- 코드 내부에 이모티콘을 사용하지 않는다
- 필요하지 않은 시스템을 미리 만들지 않는다
- 소스코드 수정 전 반드시 작업지시자 승인 요청
- 변수명은 camelcase를 따른다
- 불필요한 코드가 있다면 언급하고 삭제할지 승인 요청

---

## 폴더 규칙

- 스크립트는 반드시 Scripts 폴더 아래에 위치한다
- 프리팹은 반드시 Prefabs 폴더에 위치한다
- UI 프리팹은 Resources/UI/Popup 경로를 사용한다
- 클래스 이름과 프리팹 이름은 반드시 동일하게 유지한다
- 기능별로 폴더를 분리하고 혼합하지 않는다
- 테스트용 파일은 별도 폴더를 만들지 않는다
- 임시 파일은 생성하지 않는다

---

## 네이밍 규칙

- 클래스 이름은 PascalCase 사용
- 변수는 camelCase 사용
- UI 클래스는 "UI" 접두어 사용 (예: UINightPopup)
- Manager 클래스는 "Manager" 접미어 사용

---

## 아키텍처 구조

### Managers 구조
- Managers.cs (싱글톤)
  - GameManager
  - CustomerManager
  - UIManager

---


### UI 구조

#### UI_Base
- enum 기반 UI 바인딩

#### UI_Popup
- 모든 팝업 UI의 부모 클래스

#### UIManager
- 팝업 생성 및 제거 관리

---

## UI 규칙

팝업은 반드시 아래 경로에 위치한다

Resources/UI/Popup/

클래스 이름과 프리팹 이름은 동일해야 한다

예시:
- UINightPopup.cs
- UINightPopup.prefab

---

## UI 바인딩 규칙

코드:

```csharp
private enum Buttons
{
    StartDayButton
}

private enum Texts
{
    TitleText,
    DescText
}