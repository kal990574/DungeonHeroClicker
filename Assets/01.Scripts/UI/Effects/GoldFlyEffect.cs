using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace _01.Scripts.UI.Effects
{
    /// <summary>
    /// 몬스터 처치 시 코인이 골드 UI로 날아가는 효과.
    /// </summary>
    public class GoldFlyEffect : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _coinPrefab;

        [Header("Target")]
        [SerializeField] private RectTransform _goldUITarget;
        [SerializeField] private Canvas _canvas;

        [Header("Animation")]
        [SerializeField] private float _flyDuration = 0.5f;
        [SerializeField] private Ease _flyEase = Ease.InOutQuad;
        [SerializeField] private int _coinCount = 5;
        [SerializeField] private float _spreadRadius = 50f;
        [SerializeField] private float _delayBetweenCoins = 0.03f;
        [SerializeField] private float _arcHeight = 100f;

        private Camera _mainCamera;
        private RectTransform _canvasRect;

        private void Awake()
        {
            _mainCamera = Camera.main;

            if (_canvas != null)
            {
                _canvasRect = _canvas.GetComponent<RectTransform>();
            }
        }

        public void Play(Vector3 worldPosition, int goldAmount)
        {
            if (_coinPrefab == null || _goldUITarget == null || _canvas == null)
            {
                return;
            }

            // 월드 좌표를 스크린 좌표로 변환.
            Vector2 screenPos = _mainCamera.WorldToScreenPoint(worldPosition);

            // 스크린 좌표를 캔버스 로컬 좌표로 변환.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                screenPos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 startPos
            );

            // 코인 개수 결정 (골드 양에 따라 조절 가능).
            int count = Mathf.Min(_coinCount, Mathf.Max(1, goldAmount / 10));

            for (int i = 0; i < count; i++)
            {
                float delay = i * _delayBetweenCoins;
                SpawnAndAnimateCoin(startPos, delay);
            }
        }

        private void SpawnAndAnimateCoin(Vector2 startPos, float delay)
        {
            // 코인 스폰.
            var coin = LeanPool.Spawn(_coinPrefab, _canvas.transform);
            var coinRect = coin.GetComponent<RectTransform>();

            // 시작 위치에 랜덤 오프셋 적용.
            Vector2 randomOffset = Random.insideUnitCircle * _spreadRadius;
            Vector2 spawnPos = startPos + randomOffset;
            coinRect.anchoredPosition = spawnPos;
            coinRect.localScale = Vector3.one;

            // 목표 위치 (월드 좌표를 캔버스 로컬 좌표로 변환).
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                RectTransformUtility.WorldToScreenPoint(
                    _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                    _goldUITarget.position
                ),
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 targetPos
            );

            // 곡선 경로 애니메이션.
            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(delay);

            // 위로 살짝 올라갔다가 타겟으로 이동.
            Vector2 controlPoint = (spawnPos + targetPos) / 2f + Vector2.up * _arcHeight;

            sequence.Append(
                DOTween.To(
                    () => 0f,
                    t =>
                    {
                        // 베지에 곡선 보간.
                        Vector2 pos = CalculateBezierPoint(t, spawnPos, controlPoint, targetPos);
                        coinRect.anchoredPosition = pos;
                    },
                    1f,
                    _flyDuration
                ).SetEase(_flyEase)
            );

            // 도착 시 스케일 축소.
            sequence.Join(
                coinRect.DOScale(0.5f, _flyDuration).SetEase(Ease.InQuad)
            );

            sequence.OnComplete(() =>
            {
                LeanPool.Despawn(coin);
            });
        }

        private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            // 2차 베지에 곡선: B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
            float u = 1f - t;
            return u * u * p0 + 2f * u * t * p1 + t * t * p2;
        }
    }
}
