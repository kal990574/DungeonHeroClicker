using Lean.Pool;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
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