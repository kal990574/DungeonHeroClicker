using Lean.Pool;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    /// <summary>
    /// 몬스터 사망 시 파티클 이펙트를 스폰하는 피드백.
    /// </summary>
    public class DeathParticleFeedback : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ParticleSystem _particlePrefab;

        public void Play(Vector3 position)
        {
            if (_particlePrefab == null)
            {
                return;
            }

            var particle = LeanPool.Spawn(_particlePrefab, position, Quaternion.identity);
            particle.Play();

            var duration = particle.main.duration + particle.main.startLifetime.constantMax;
            LeanPool.Despawn(particle, duration);
        }
    }
}