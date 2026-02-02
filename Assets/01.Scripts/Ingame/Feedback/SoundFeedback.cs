using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class SoundFeedback : MonoBehaviour, IFeedback
    {
        [Header("Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private bool _randomPitch;
        [SerializeField] private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);

        public void Play(Vector3 position)
        {
            if (_audioSource == null || _clip == null)
            {
                return;
            }

            if (_randomPitch)
            {
                _audioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
            }

            _audioSource.PlayOneShot(_clip, _volume);
        }

        public void Stop()
        {
            if (_audioSource != null && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}