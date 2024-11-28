# 2048 Game
2048 게임

<img src="https://github.com/user-attachments/assets/2b441384-75b3-4573-bfa7-4f1e69de0125" width="250"></img>
<img src="https://github.com/user-attachments/assets/785acccd-2ff3-463d-a451-9730e9d1f9f2" width="250"></img>
<img src="https://github.com/user-attachments/assets/0fb15c14-b487-40f6-aeda-ca821a1c5ecc" width="250"></img>
</br>

## 1. 게임 메커니즘
  - 플레이어가 타일을 상,하,좌,우로 움직이며 같은 숫자가 적힌 두 타일을 합쳐서 새로운 타일을 만들며 점수를 획득하는 게임
## 2. 주요 목표
  - 합칠때마다 점수를 획득하여 높은 점수를 기록
  - "2048" 타일을 만드는 것
## 3. 개발 환경
  - Unity, C#
</br>

## 개발 일지
[2024-11-18]
- Project Setting
- Add
  - Main Grid 제작
  - Tile Setting (Moving, Animation)
  - Tile Merging System (Merge, Animation)
    - Error : 한 입력에 같은라인의 merge 현상이 여러번 되는 현상
      - 해결 : bool타입의 lock으로 판별
  - GameOver System
  - Score System
  - 전체적인 UI
</br>

[2024-11-19]
- Add
  - ScorePopup UI
- Build Test Complete
</br>

[2024-11-20]
- 제작 완료
</br>
