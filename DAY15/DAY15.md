# DAY 15: 네트워크 게임 통합과 최종 점검

## 최종 실행 결과

- Host 시작 성공
- Client 연결 성공
- Player 2명 생성 확인
- Host / Client 각각 자신의 Player 이동 확인
- Server에서 Coin 획득 판정
- Coin 획득 시 양쪽 화면에서 Coin 제거 확인
- NetworkVariable을 통한 Score 동기화 확인
- Client 종료 후 Host 유지 확인
- Client 재접속 후 Player 재생성 확인
- UniTask를 이용한 Host 시작 상태 UI 확인
- Host 중복 시작 방지 확인
- 실패 상황에서 Retry 버튼 표시 확인

## 협동 목표

Coin 3개를 획득하면 공유 목표가 완료되도록 구현했다.

실행 흐름:

```text
Coins: 0 / 3
→ Coins: 1 / 3
→ Coins: 2 / 3
→ GAME SUCCESS