# DAY02 - 네트워크 게임 서버와 프로토콜 설계

## 1. 코인 수집 게임 서버 구성

구성 요소

- Player A Client
- Player B Client
- Game Server
- Database

데이터 흐름

Player A Client → Game Server
- MoveRequest
- CollectCoinRequest

Player B Client → Game Server
- MoveRequest
- CollectCoinRequest

Game Server → Player A Client
- PlayerStateChanged
- CoinCollected

Game Server → Player B Client
- PlayerStateChanged
- CoinCollected

Game Server → Database
- SaveScore


## 2. 요청과 판정

Client는 플레이어의 이동과 코인 획득을 요청한다.

Game Server는 이동 가능 여부, 코인과 플레이어의 거리,
코인이 이미 획득되었는지 등을 검사하여 최종 결과를 결정한다.

점수 증가와 코인 제거는 Client가 아니라 Game Server에서 결정한다.

Game Server에서 결정된 결과는 Player A와 Player B에게 전달되어
두 Client가 같은 게임 상태를 확인할 수 있게 한다.

Database는 실시간 이동이나 코인 판정을 담당하지 않고,
서버에서 확정된 점수와 운영 데이터를 저장하는 역할을 담당한다.


## 3. 메시지 명세

| 메시지 이름 | 방향 | 필드 | Server 처리 |
| --- | --- | --- | --- |
| CollectCoinRequest | Client → Server | playerId, coinId | 플레이어와 코인의 거리 및 이미 획득된 코인인지 검사 |
| CoinCollected | Server → All Clients | playerId, coinId, score | 코인을 제거하고 확정된 점수를 모든 Client에 전달 |

### CollectCoinRequest에 score를 넣지 않는 이유

점수는 Client가 임의로 결정하는 값이 아니라
Game Server가 코인 획득 조건을 검사한 뒤 계산하고 확정해야 하는 값이기 때문이다.


## 4. 모듈 명세

### NetworkInputModule

Client에서 전달된 MoveRequest, CollectCoinRequest 등의
네트워크 요청을 수신하고 필요한 데이터를 해석한다.

### GameRuleModule

플레이어 이동, 코인과의 거리, 코인 획득 여부,
점수 증가와 같은 게임 규칙을 검사하고 최종 결과를 결정한다.

### DatabaseModule

Game Server에서 확정된 점수와 필요한 운영 데이터를
Database에 저장하거나 불러오는 역할을 담당한다.

### UiModule

Server가 전달한 PlayerStateChanged, CoinCollected 등의 결과를 받아
Client 화면의 플레이어 상태, 코인, 점수 UI를 갱신한다.


## 5. 오류 메시지

잘못된 coinId가 들어온 경우:

InvalidCoinId

Server는 존재하지 않거나 유효하지 않은 coinId를 확인하면
해당 요청을 정상적인 코인 획득으로 처리하지 않는다.


## 6. 서버 권한 정리

- Client는 행동을 요청한다.
- Server는 요청이 가능한지 검사한다.
- 점수와 코인 획득 결과는 Server가 최종 결정한다.
- 확정된 결과를 모든 Client에게 전달한다.
- Database는 확정된 데이터를 저장한다.


## 7. 완료 확인

- [x] 점수와 코인 제거를 Game Server가 결정하도록 설계했다.
- [x] Player A와 Player B가 같은 결과를 받도록 설계했다.
- [x] Database를 실시간 이동 처리와 분리했다.
- [x] 요청 메시지와 결과 메시지를 구분했다.
- [x] Server가 검사할 조건을 작성했다.
- [x] 각 모듈의 중심 책임을 구분했다.

## 8. 응용 실습 - 협동 보스전

### 서버 구성

구성 요소

- Player A Client
- Player B Client
- Game Server
- Database

데이터 흐름

Player A Client → Game Server
- MoveRequest
- AttackRequest

Player B Client → Game Server
- MoveRequest
- AttackRequest

Game Server → Player A Client
- PlayerStateChanged
- BossStateChanged
- DamageResult

Game Server → Player B Client
- PlayerStateChanged
- BossStateChanged
- DamageResult

Game Server → Database
- SaveBattleResult


### 요청과 판정

Client는 이동과 공격을 Game Server에 요청한다.

Game Server는 플레이어의 위치, 공격 가능 거리, 보스의 현재 상태를 확인한 뒤
공격 성공 여부와 피해량을 최종 결정한다.

보스 HP와 피해량은 Client가 직접 결정하지 않는다.

Game Server에서 확정된 보스 상태와 전투 결과는
Player A와 Player B에게 동일하게 전달한다.

Database는 실시간 공격 판정을 담당하지 않고
전투가 끝난 뒤 확정된 결과를 저장한다.


### 메시지 명세

| 메시지 이름 | 방향 | 필드 | Server 처리 |
| --- | --- | --- | --- |
| AttackRequest | Client → Server | playerId, bossId, skillId | 플레이어 상태, 공격 거리, 스킬 사용 가능 여부 검사 |
| DamageResult | Server → All Clients | playerId, bossId, damage | 확정된 피해량 전달 |
| BossStateChanged | Server → All Clients | bossId, hp | 확정된 보스 HP를 모든 Client에 전달 |
| SaveBattleResult | Server → Database | bossId, clearResult | 전투 종료 결과 저장 |


### Server가 반드시 판정해야 할 데이터 3개

1. 공격 성공 여부
2. 보스에게 적용되는 피해량
3. 보스의 최종 HP와 처치 여부


### 서버 권한 정리

- Client는 공격을 요청한다.
- Server는 공격 조건을 검사한다.
- 피해량과 보스 HP는 Server가 최종 결정한다.
- 확정된 결과를 두 Client에게 전달한다.
- 전투 종료 결과는 Database에 저장한다.




