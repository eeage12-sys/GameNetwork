# DAY 14: 멀티플레이 테스트 환경과 튜닝

## 1. 테스트 환경

- Scene: NetworkPlay
- Host: Unity Editor
- Client: Windows Development Build
- 테스트 인원: 2명
- Network 방식: Host / Client
- 테스트 기능:
  - Host 시작
  - Client 연결
  - Player 생성
  - Coin 획득
  - Score 동기화
  - Client 종료
  - Client 재접속

---

## 2. Host / Client 통합 테스트

| 번호 | 절차 | 기대 결과 | 실제 결과 |
| --- | --- | --- | --- |
| 1 | Host 시작 | Host Player가 생성된다. | 성공 - Host 시작 후 Player 1개 생성 확인 |
| 2 | Client 연결 | 양쪽에 Player 2개가 보인다. | 성공 - Client 연결 후 Host에 Players 2 표시 및 Player 2개 생성 확인 |
| 3 | 코인 획득 | 코인이 양쪽에서 한 번 사라지고 점수가 동기화된다. | 성공 - Coin 획득 후 Coin 제거 및 Score 증가 확인 |
| 4 | Client 종료 | Host에서 Client 종료를 확인하고 남은 Player가 유지된다. | 성공 - Client 종료 후 Players 2 → 1로 감소하고 Host Player가 유지됨. Console 종료 로그는 별도 확인하지 않음 |
| 5 | 다시 연결 | 새 Player가 생성되고 현재 공유 상태를 받는다. | 성공 - Client 재접속 후 새 Player가 생성되고 Players 2로 복구됨 |

---

## 3. 테스트 과정 확인

### Host 시작

Host Start 버튼을 눌렀을 때 Host가 정상적으로 실행되었다.

확인 결과:

```text
Host Running / Players 1
Host Start Complete