using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class ParticleFeedback : MonoBehaviour, IFeedback
    {
        [Header("Settings")]
        [SerializeField] private ParticleSystem _particlePrefab;
        [SerializeField] private bool _usePooling;

        public void Play(Vector3 position)
        {
            if (_particlePrefab == null)
            {
                return;
            }

            // TODO: 풀링 적용 시 ObjectPool에서 가져오기
            var particle = Instantiate(_particlePrefab, position, Quaternion.identity);
            particle.Play();

            var duration = particle.main.duration + particle.main.startLifetime.constantMax;
            Destroy(particle.gameObject, duration);
        }

        public void Stop()
        {
            // 파티클은 자동 소멸하므로 별도 처리 불필요
        }
    }
}