# 인벤토리 감시 시스템

게임 인벤토리의 아이템 수량이 변경될 때 구독자들에게 알리는 시스템을 구현하세요.

**요구사항**

1. `Inventory` 클래스 (게시자)
   - `event Action<string, int, int> ItemChanged` 이벤트 선언
	(매개변수: 아이템명, 이전 수량, 현재 수량)
   - 내부에 `Dictionary<string, int>`로 아이템 저장소 관리
   - `AddItem(string name, int count)` 메서드: 아이템 추가 (기존 아이템이면 수량 증가)
   - `RemoveItem(string name, int count)` 메서드: 아이템 제거 (수량 차감, 0 미만 방지)

2. `InventoryUI` 클래스 (구독자)
   - 모든 변경 내역을 "[UI] {name}: {oldCount} → {newCount}" 형식으로 출력

3. `AutoBuyer` 클래스 (구독자)
   - 아이템 수량이 0이 되면 "[자동구매] {name} 재고 소진! 자동 구매 요청" 출력

4. Main에서 테스트
   - `Inventory`, `InventoryUI`, `AutoBuyer` 인스턴스 생성 및 이벤트 구독
   - 아이템 추가/제거 시나리오 실행

## 예상 실행 결과

```
[UI] 포션: 0 → 5
[UI] 화살: 0 → 10
[UI] 포션: 5 → 8
[UI] 화살: 10 → 3
[UI] 화살: 3 → 0
[자동구매] 화살 재고 소진! 자동 구매 요청
```
