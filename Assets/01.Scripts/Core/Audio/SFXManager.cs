using UnityEngine;

namespace _01.Scripts.Core.Audio
{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager Instance { get; private set; }

        [Header("Audio")] [SerializeField] private AudioSource _audioSource;

        [Header("Clips")] [SerializeField] private AudioClip[] _hitClips;
        [SerializeField] private AudioClip[] _coinClips;
        [SerializeField] private AudioClip[] _uiClips;
        [SerializeField] private AudioClip[] _upgradeClips;

        [Header("Settings")] [SerializeField] private float _volume = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.playOnAwake = false;
        }

        public void PlayHit()
        {
            PlayRandom(_hitClips);
        }

        public void PlayDeath()
        {
            PlayRandom(_coinClips);
        }
        public void PlayCoin()
        {
            PlayRandom(_coinClips);
        }

        public void PlayUI()
        {
            PlayRandom(_uiClips);
        }

        public void PlayUpgrade()
        {
            PlayRandom(_upgradeClips);
        }

        public void SetVolume(float volume)
        {
            _volume = Mathf.Clamp01(volume);
        }
        
        private void PlayRandom(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
            {
                return;
            }
            
            var clip = clips[Random.Range(0, clips.Length)];
            _audioSource.PlayOneShot(clip, _volume);
        }
    }
}