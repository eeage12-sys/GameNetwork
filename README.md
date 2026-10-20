# GameNetwork

게임 네트워크 프로그래밍 실습 프로젝트입니다.

## 프로젝트 구성

- DAY01 ~ DAY08 : C# 네트워크 기초 및 TCP/UDP 실습
- DAY05/UdpPeerLab : UDP Peer 프로그램
- DAY13 : 서버 데이터, 리포트, 패치 설계
- DAY14 : Host / Client 통합 테스트 기록
- DAY15 : Unity 네트워크 게임 최종 점검
- GameNetworkUnity : Unity Netcode 기반 네트워크 게임

## Unity 네트워크 게임

Unity Netcode for GameObjects를 사용하여 Host / Client 방식의
간단한 멀티플레이 게임을 구현했습니다.

주요 기능:

- Host / Client 연결
- 네트워크 Player 생성
- Player 이동
- Server 기반 Coin 획득 판정
- Score 동기화
- Coin 상태 동기화
- Client 종료 및 재접속
- UniTask를 이용한 연결 상태 UI
- Coin 3개 획득 시 GAME SUCCESS 동기화

## Unity 실행 방법

Unity 프로젝트:

GameNetworkUnity

Scene:

NetworkPlay

Windows Client Build:

GameNetworkUnity/Builds/NetworkLab/GameNetworkUnity.exe

테스트 방법:

1. Unity Editor에서 NetworkPlay Scene을 실행합니다.
2. Host Start 버튼을 누릅니다.
3. GameNetworkUnity.exe를 실행합니다.
4. Client Start 버튼을 누릅니다.
5. Host 화면에 Players 2가 표시되는지 확인합니다.
6. Player를 이동하여 Coin 획득 및 Score 동기화를 확인합니다.
7. Coin 3개 획득 시 GAME SUCCESS가 양쪽에 표시되는지 확인합니다.

## UDP Peer 프로그램

위치:

DAY05/UdpPeerLab

주요 파일:

- Program.cs
- UdpPeerLab.csproj

터미널에서 해당 폴더로 이동한 뒤 다음 명령으로 실행할 수 있습니다.

dotnet run

## 테스트 문서

- DAY13/DAY13.md
- DAY14/DAY14.md
- DAY15/DAY15.md

DAY14 문서에는 Host / Client 연결, Coin 획득,
Client 종료 및 재접속 테스트 결과를 기록했습니다.

DAY15 문서에는 최종 네트워크 게임 통합 결과와
GAME SUCCESS 협동 목표 구현 내용을 기록했습니다.