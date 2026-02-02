using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01.Scripts.UI.Effects
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private TMP_Text _floatingTextPrefab;

        [Header("Canvas")]
        [SerializeField] private Canvas _targetCanvas;

        [Header("Animation")]
        [SerializeField] private float _floatDistance = 50f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeEase = Ease.InQuad;

        [Header("Pool")]
        [SerializeField] private int _initialPoolSize = 5;

        private readonly Queue<TMP_Text> _pool = new Queue<TMP_Text>();
        private Transform _poolParent;

        private void Awake()
        {
            FindCanvas();
            InitializePool();
        }

        private void FindCanvas()
        {
            if (_targetCanvas == null)
            {
                _targetCanvas = GetComponentInParent<Canvas>();
            }

            if (_targetCanvas == null)
            {
                _targetCanvas = FindFirstObjectByType<Canvas>();
            }

            _poolParent = _targetCanvas != null ? _targetCanvas.transform : transform;
        }

        private void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                var instance = CreateInstance();
                instance.gameObject.SetActive(false);
                _pool.Enqueue(instance);
            }
        }

        private TMP_Text CreateInstance()
        {
            var instance = Instantiate(_floatingTextPrefab, _poolParent);
            return instance;
        }

        private TMP_Text GetFromPool()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }

            return CreateInstance();
        }

        private void ReturnToPool(TMP_Text instance)
        {
            instance.gameObject.SetActive(false);
            _pool.Enqueue(instance);
        }

        public void Spawn(string text, Vector3 worldPosition, Color color)
        {
            var instance = GetFromPool();

            instance.text = text;
            instance.color = color;
            instance.transform.position = worldPosition;
            instance.gameObject.SetActive(true);

            PlayAnimation(instance);
        }

        public void SpawnAtUI(string text, RectTransform targetUI, Color color)
        {
            SpawnAtUI(text, targetUI, color, Vector2.zero);
        }

        public void SpawnAtUI(string text, RectTransform targetUI, Color color, Vector2 offset)
        {
            var instance = GetFromPool();

            instance.text = text;
            instance.color = color;

            // Canvas 하위에서 올바른 위치 설정.
            instance.rectTransform.position = targetUI.position;
            instance.rectTransform.anchoredPosition += offset;
            instance.rectTransform.SetAsLastSibling();
            instance.gameObject.SetActive(true);

            PlayAnimation(instance);
        }

        private void PlayAnimation(TMP_Text instance)
        {
            var rectTransform = instance.rectTransform;
            var startPosition = rectTransform.anchoredPosition;
            var endPosition = startPosition + Vector2.up * _floatDistance;

            instance.alpha = 1f;

            var sequence = DOTween.Sequence();

            sequence.Append(
                rectTransform.DOAnchorPos(endPosition, _duration).SetEase(_moveEase)
            );

            sequence.Join(
                instance.DOFade(0f, _duration).SetEase(_fadeEase)
            );

            sequence.OnComplete(() =>
            {
                rectTransform.anchoredPosition = startPosition;
                ReturnToPool(instance);
            });
        }
    }
}