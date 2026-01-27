using System;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01.Scripts.UI.Effects
{
    /// <summary>
    /// 몬스터 처치 시 코인이 파편처럼 터진 후 골드 UI로 날아가는 효과.
    /// </summary>
    public class GoldFlyEffect : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _coinPrefab;

        [Header("Target")]
        [SerializeField] private RectTransform _goldUITarget;
        [SerializeField] private Canvas _canvas;

        [Header("Coin Count")]
        [SerializeField] private int _coinCount = 5;

        [Header("Phase 1 - Explosion")]
        [SerializeField] private float _explosionDuration = 0.2f;
        [SerializeField] private float _explosionRadius = 100f;
        [SerializeField] private Ease _explosionEase = Ease.OutQuad;

        [Header("Phase 2 - Fly to UI")]
        [SerializeField] private float _flyDuration = 0.4f;
        [SerializeField] private Ease _flyEase = Ease.InQuad;
        [SerializeField] private float _delayBetweenCoins = 0.03f;
        [SerializeField] private float _arcHeight = 50f;

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

        public void Play(Vector3 worldPosition, int goldAmount, Action<int> onCoinArrived = null)
        {
            if (_coinPrefab == null || _goldUITarget == null || _canvas == null)
            {
                return;
            }

            // 월드 좌표를 캔버스 로컬 좌표로 변환.
            Vector2 screenPos = _mainCamera.WorldToScreenPoint(worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                screenPos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 startPos
            );

            // 코인당 골드 계산.
            int goldPerCoin = goldAmount / _coinCount;
            int remainder = goldAmount % _coinCount;

            for (int i = 0; i < _coinCount; i++)
            {
                float delay = i * _delayBetweenCoins;
                int coinGold = goldPerCoin + (i == 0 ? remainder : 0);
                SpawnAndAnimateCoin(startPos, delay, coinGold, onCoinArrived);
            }
        }

        private void SpawnAndAnimateCoin(Vector2 startPos, float delay, int goldValue, Action<int> onArrived)
        {
            // 코인 스폰.
            var coin = LeanPool.Spawn(_coinPrefab, _canvas.transform);
            var coinRect = coin.GetComponent<RectTransform>();

            coinRect.anchoredPosition = startPos;
            coinRect.localScale = Vector3.one;

            // 목표 위치 계산.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasRect,
                RectTransformUtility.WorldToScreenPoint(
                    _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                    _goldUITarget.position
                ),
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 targetPos
            );

            // 파편 방향 (랜덤 각도).
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float radius = Random.Range(_explosionRadius * 0.8f, _explosionRadius * 1.2f);
            Vector2 explosionPos = startPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            // 애니메이션 시퀀스.
            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(delay);

            // Phase 1: 파편처럼 퍼짐.
            sequence.Append(
                coinRect.DOAnchorPos(explosionPos, _explosionDuration).SetEase(_explosionEase)
            );

            // Phase 2: UI로 곡선 이동.
            Vector2 controlPoint = (explosionPos + targetPos) / 2f + Vector2.up * _arcHeight;

            sequence.Append(
                DOTween.To(
                    () => 0f,
                    t =>
                    {
                        Vector2 pos = CalculateBezierPoint(t, explosionPos, controlPoint, targetPos);
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

            // 도착 완료.
            sequence.OnComplete(() =>
            {
                onArrived?.Invoke(goldValue);
                LeanPool.Despawn(coin);
            });
        }

        private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1f - t;
            return u * u * p0 + 2f * u * t * p1 + t * t * p2;
        }
    }
}