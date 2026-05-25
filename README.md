# 🏰 Dungeon Hero Clicker

  > 클릭으로 성장하는 2D 방치형 RPG                                                                                 
   
  [![게임 플레이 영상](https://img.youtube.com/vi/6iUDAm97RrA/0.jpg)](https://youtube.com/shorts/6iUDAm97RrA)
                  
  ## 📋 프로젝트 개요                                                                                               
                  
  | 항목 | 내용 |                                                                                                   
  |:---:|:---|    
  | **개발 기간** | 2026.01.26 ~ 2026.01.28 (42시간 게임잼) + 2026.01.29 ~ 2026.02.09 (아키텍처 리팩토링) |
  | **팀 구성** | 1인 개발 |                                                                                        
  | **플랫폼** | WebGL + Desktop (크로스 플랫폼) |                                                                  
  | **장르** | 2D Idle Clicker (방치형 클리커) |                                                                    
  | **담당 역할** | 전체 설계 및 구현 (코어 게임루프, 피드백 시스템, 클린 아키텍처, 크로스플랫폼 저장소) |          
                                                                                                                    
  ## 🎮 게임 소개                                                                                                   
                                                                                                                    
  몬스터를 클릭해 골드를 획득하고, 무기 강화와 동료 고용으로 자동 DPS 체계를 구축하며 무한 스테이지를 돌파하는 2D   
  방치형 클리커 게임입니다.
                                                                                                                    
  클릭 데미지와 크리티컬 시스템으로 타격 쾌감을 제공하고, 동료(Knight, Archer, Mage, Reaper)를 고용하여 방치 수익을 
  구축하세요.
                                                                                                                    
  ## ⭐ 주요 기능 및 콘텐츠

  ### 🗡 클릭 & 크리티컬 시스템                                                                                     
  - Raycast 기반 클릭 입력, 50% 크리티컬 확률 + 2배 데미지 배율
  - 크리티컬 시 Camera Shake + 확대 팝업으로 차등 피드백                                                            
                                                                                                                    
  ### 👫 동료 자동 DPS                                                                                              
  - 동료 고용 시 자동 DPS 생성, 1초 간격 자동 타격                                                                  
  - 고용한 동료들의 DPS 합산으로 방치 수익 구축                                                                     
                                                                                                                    
  ### 📈 BigNumber 무한 성장                                                                                        
  - 과학적 표기법 기반 BigNumber 구조체 (`Mantissa × 10^Exponent`)                                                  
  - UI에서 "1.23K", "4.56M", "7.89B" 등 축약 표기 지원                                                              
                                                                                                                    
  ### 🏔 스테이지 무한 진행                                                                                         
  - 스테이지별 몬스터 체력/보상 지수 스케일링                                                                       
  - 스테이지 마지막 몬스터 보스 시스템                                                                              
                                                                                                                    
  ### ✨ 피드백 시스템 (IFeedback 패턴)                                                                          
  - Scale Punch, Color Flash, Camera Shake, Damage Popup 조합형 피드백                                              
  - DOTween 기반 트윈 중첩 방지 (Play 전 Kill + 원본 복원)                                                          
                                                                                                                    
  ### 🏗️클린 아키텍처 (3계층 구조)                                                                                 
  - Manager → Domain → Repository 레이어 분리                                                                       
  - Domain: 비즈니스 규칙 캡슐화, 생성자 검증, With 패턴 상태 전이                                                  
  - Repository: 인터페이스 기반 저장소 추상화                                                                       
                                                                                                                    
  ### 🌐 크로스 플랫폼 저장소                                                                                       
  - `ICurrencyRepository` / `IUpgradeRepository` / `IStageRepository` 인터페이스                                    
  - WebGL: 로컬 JSON + IndexedDB 동기화                                                                             
  - Desktop: Firebase Firestore 클라우드 저장                                                                       
  - Manager의 Awake()에서 `#if UNITY_WEBGL` 한 곳에만 분기 격리                                                     
                                                                                                                    
  ## 🕹 조작법                                                                                                      
                                                                                                                    
  | 입력 | 동작 |                                                                                                   
  |:---:|:---|
  | `마우스 클릭` | 몬스터 타격 (수동 데미지) |                                                                     
  | `업그레이드 패널` | 무기 강화 / 동료 고용 · 업그레이드 |                                                        
                                                                                                                    
  ## 🛠️기술 스택                                                                                                   
                                                                                                                    
  | 항목 | 내용 | 
  |:---:|:---|
  | **Engine** | Unity 6 (6000.0.60f1) |                                                                            
  | **Language** | C# |
  | **Rendering** | URP (Universal Render Pipeline) |                                                               
  | **주요 패키지** | UniTask, DOTween, Firebase Auth/Firestore |
  | **버전 관리** | Git + GitHub |                                                                                  
  | **CI/CD** | GitHub Actions (WebGL 빌드 + GitHub Pages 배포 + Slack 알림) |
                                                                                                                    
  > **Note:** 42시간 게임잼으로 코어 루프를 완성한 뒤, 클린 아키텍처와 크로스플랫폼 저장소를 학습·적용한            
  프로젝트입니다.    
