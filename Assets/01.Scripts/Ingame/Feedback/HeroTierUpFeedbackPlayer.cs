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
        [SerializeField] private bool _useScreenCenter = true;
        [SerializeField] private Vector3 _textOffset = new Vector3(0f, 0f, 0f);

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
                var textPosition = _useScreenCenter ? GetScreenCenterWorld() : position;
                _textFeedback.Play(textPosition + _textOffset);
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

        private Vector3 GetScreenCenterWorld()
        {
            var cam = Camera.main;
            if (cam == null) return transform.position;

            var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 10f);
            return cam.ScreenToWorldPoint(screenCenter);
        }
    }
}