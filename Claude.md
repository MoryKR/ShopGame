# ShopGame
ShopGame
# 프로젝트 개발 규칙 (CLAUDE.md)

## 프로젝트 개요
이 프로젝트는 Unity로 제작하는 2D 아이소메트릭 상점 운영 게임이다.

플레이어는 상점을 운영하며, 손님이 등장하고, 상점 내부를 돌아다니다가 계산대로 이동하고, 말풍선을 표시한 뒤 결제를 하고 나가는 흐름을 가진다.

이 프로젝트는 Claude Desktop / Claude Code를 활용한 AI 협업 기반으로 개발되며, 코드 구조와 설계는 포트폴리오 수준으로 유지한다.

---

## 사용 기술
- Unity 2D
- C#
- NavMesh 기반 이동
- Popup 기반 UI 구조

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

---

## 폴더 구조 규칙

모든 에셋과 스크립트는 아래 구조를 반드시 따른다.

### 기본 구조

Assets/
 ├── Scripts/
 ├── Art/
 ├── Prefabs/
 ├── Scenes/
 ├── UI/
 ├── Resources/
 └── Settings/

---

### Scripts 폴더

코드는 기능별로 분리한다.

Assets/Scripts/
 ├── Managers/
 │    ├── Managers.cs
 │    ├── GameManager.cs
 │    ├── CustomerManager.cs
 │    └── UIManager.cs
 │
 ├── Customer/
 │    ├── Customer.cs
 │    └── CustomerBubbleUI.cs
 │
 ├── UI/
 │    ├── UI_Base.cs
 │    ├── UI_Popup.cs
 │    └── Popup/
 │         └── UINightPopup.cs
 │
 └── Utils/
      └── (공통 유틸 스크립트)

---

### Art 폴더

이미지 및 리소스 파일은 종류별로 분리한다.

Assets/Art/
 ├── Characters/
 ├── Environment/
 └── UI/

---

### Prefabs 폴더

프리팹은 역할별로 분리한다.

Assets/Prefabs/
 ├── Characters/
 │    └── Customer/
 ├── UI/
 └── Environment/

---

### Scenes 폴더

씬 파일을 관리한다.

Assets/Scenes/
 └── Main.unity

---

### UI 폴더

UI 관련 리소스를 관리한다.

Assets/UI/
 ├── Popup/
 └── Common/

---

### Resources 폴더

런타임에서 로드되는 프리팹은 반드시 Resources 폴더에 둔다.

Assets/Resources/
 └── UI/
      └── Popup/
           └── UINightPopup.prefab

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

### GameManager
- 골드 관리
- 낮 / 밤 상태 관리
- 낮 시간 타이머

---

### CustomerManager
- 손님 스폰
- 최대 손님 수 제한 (기본 3명)
- 계산대 배정
- 손님 리스트 관리

---

### Customer
상태:
- Wander
- MovingToCounter
- WaitingAtCounter
- Leaving

동작:
- NavMesh 기반 이동
- 이동 방향에 따라 SpriteRenderer flip
- 계산대 도착 시 정지 및 오른쪽 바라보기
- ExitPoint 도착 시 CustomerManager에 제거 요청

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