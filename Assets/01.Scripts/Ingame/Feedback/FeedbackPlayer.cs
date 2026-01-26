using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [Header("Feedbacks")]
        [SerializeField] private ScaleFeedback _scaleFeedback;
        [SerializeField] private SoundFeedback _soundFeedback;
        [SerializeField] private ParticleFeedback _particleFeedback;
        [SerializeField] private DamagePopupFeedback _damagePopupFeedback;

        public void Play(Vector3 position)
        {
            _scaleFeedback?.Play(position);
            _soundFeedback?.Play(position);
            _particleFeedback?.Play(position);
        }

        public void PlayWithDamage(Vector3 position, float damage, bool isCritical = false)
        {
            Play(position);

            if (_damagePopupFeedback == null)
            {
                return;
            }

            if (isCritical)
            {
                _damagePopupFeedback.PlayCritical(position, damage);
            }
            else
            {
                _damagePopupFeedback.Play(position, damage);
            }
        }

        public void StopAll()
        {
            _scaleFeedback?.Stop();
            _soundFeedback?.Stop();
            _particleFeedback?.Stop();
            _damagePopupFeedback?.Stop();
        }
    }
}