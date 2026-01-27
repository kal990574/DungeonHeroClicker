using _01.Scripts.Core.Audio;
using _01.Scripts.Ingame.Hero;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public class HeroTierUpFeedbackPlayer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HeroVisual _heroVisual;

        [Header("Feedbacks")]
        [SerializeField] private TierUpParticleFeedback _particleFeedback;
        [SerializeField] private TierUpScaleFeedback _scaleFeedback;
        [SerializeField] private TierUpTextFeedback _textFeedback;

        [Header("Options")]
        [SerializeField] private bool _useCameraShake = true;
        [SerializeField] private Vector3 _textOffset = new Vector3(0f, 1f, 0f);

        private void OnEnable()
        {
            if (_heroVisual != null)
            {
                _heroVisual.OnTierChanged += HandleTierChanged;
            }
        }

        private void OnDisable()
        {
            if (_heroVisual != null)
            {
                _heroVisual.OnTierChanged -= HandleTierChanged;
            }
        }

        private void HandleTierChanged(TierChangeInfo info)
        {
            PlayTierUpEffect(info);
        }

        private void PlayTierUpEffect(TierChangeInfo info)
        {
            var heroTransform = _heroVisual.CurrentHero?.transform;
            var position = heroTransform != null ? heroTransform.position : transform.position;

            _particleFeedback?.Play(position);
            _scaleFeedback?.Play(position);

            if (_textFeedback != null)
            {
                _textFeedback.SetTierName(info.NewTierName);
                _textFeedback.Play(position + _textOffset);
            }

            if (_useCameraShake)
            {
                CameraShakeFeedback.Shake();
            }

            SFXManager.Instance?.PlayTierUp();
        }

        public void StopAll()
        {
            _particleFeedback?.Stop();
            _scaleFeedback?.Stop();
            _textFeedback?.Stop();
        }
    }
}