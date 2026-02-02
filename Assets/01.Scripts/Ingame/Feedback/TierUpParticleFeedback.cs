using Lean.Pool;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class TierUpParticleFeedback : MonoBehaviour, IFeedback
    {
        [Header("Settings")]
        [SerializeField] private ParticleSystem _burstParticlePrefab;

        public void Play(Vector3 position)
        {
            if (_burstParticlePrefab == null) return;

            var particle = LeanPool.Spawn(_burstParticlePrefab, position, Quaternion.identity);
            particle.Play();

            var duration = particle.main.duration + particle.main.startLifetime.constantMax;
            LeanPool.Despawn(particle, duration);
        }

        public void Stop()
        {
            // 풀링으로 관리되므로 별도 처리 불필요.
        }
    }
}