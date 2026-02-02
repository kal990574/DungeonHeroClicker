using DG.Tweening;
using UnityEngine;

namespace _01.Scripts.Core.Audio
{
    public class BGMManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _bgmClip;

        [Header("Settings")]
        [SerializeField] private float _volume = 0.5f;
        [SerializeField] private bool _playOnAwake = true;
        [SerializeField] private float _fadeDuration = 1f;

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
            _audioSource.volume = 0f;
        }

        private void Start()
        {
            if (_playOnAwake && _bgmClip != null)
            {
                Play();
            }
        }

        public void Play()
        {
            if (_bgmClip == null)
            {
                return;
            }

            _audioSource.clip = _bgmClip;
            _audioSource.Play();
            FadeIn();
        }

        public void Stop()
        {
            FadeOut(() => _audioSource.Stop());
        }

        public void SetVolume(float volume)
        {
            _volume = Mathf.Clamp01(volume);
            _audioSource.volume = _volume;
        }

        private void FadeIn()
        {
            _audioSource.volume = 0f;
            _audioSource.DOFade(_volume, _fadeDuration);
        }

        private void FadeOut(TweenCallback onComplete = null)
        {
            _audioSource.DOFade(0f, _fadeDuration).OnComplete(onComplete);
        }
    }
}