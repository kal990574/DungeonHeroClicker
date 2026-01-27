using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class TierUpParticleFeedback : MonoBehaviour, IFeedback
    {
        [Header("Particles")]
        [SerializeField] private ParticleSystem _burstParticle;
        [SerializeField] private ParticleSystem _auraParticle;
        [SerializeField] private ParticleSystem _riseParticle;

        public void Play(Vector3 position)
        {
            PlayParticle(_burstParticle, position);
            PlayParticle(_auraParticle, position);
            PlayParticle(_riseParticle, position);
        }

        public void Stop()
        {
            StopParticle(_burstParticle);
            StopParticle(_auraParticle);
            StopParticle(_riseParticle);
        }

        private void PlayParticle(ParticleSystem particle, Vector3 position)
        {
            if (particle == null) return;

            particle.transform.position = position;
            particle.Play();
        }

        private void StopParticle(ParticleSystem particle)
        {
            if (particle == null) return;

            particle.Stop();
            particle.Clear();
        }
    }
}