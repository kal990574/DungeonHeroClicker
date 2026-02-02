using _01.Scripts.Core.Audio;
using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [Header("Feedbacks")]
        [SerializeField] private ScaleFeedback _scaleFeedback;
        [SerializeField] private ParticleFeedback _particleFeedback;
        [SerializeField] private DamagePopupFeedback _damagePopupFeedback;
        [SerializeField] private ColorFlashFeedback _colorFlashFeedback;

        public void Play(Vector3 position)
        {
            _scaleFeedback?.Play(position);
            SFXManager.Instance.PlayHit();
            _particleFeedback?.Play(position);
            _colorFlashFeedback?.Play(position);
        }

        public void PlayWithDamage(Vector3 position, BigNumber damage, bool isCritical = false)
        {
            Play(position);

            if (_damagePopupFeedback == null)
            {
                return;
            }

            if (isCritical)
            {
                _damagePopupFeedback.PlayCritical(position, damage);
                CameraShakeFeedback.Shake();
            }
            else
            {
                _damagePopupFeedback.Play(position, damage);
            }
        }

        public void StopAll()
        {
            _scaleFeedback?.Stop();
            _particleFeedback?.Stop();
            _damagePopupFeedback?.Stop();
            _colorFlashFeedback?.Stop();
        }
    }
}