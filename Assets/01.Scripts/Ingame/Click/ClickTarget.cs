using UnityEngine;
using System;
using _01.Scripts.Interfaces;

namespace _01.Scripts.Ingame.Click
{
    public class ClickTarget : MonoBehaviour, IClickable
    {
        [Header("Feedback")]
        [SerializeField] private ClickFeedback _clickFeedback;

        public event Action OnClicked;

        private bool _isClickable = true;

        public bool IsClickable => _isClickable;

        public void OnClick()
        {
            if (!_isClickable)
            {
                return;
            }

            OnClicked?.Invoke();
            PlayFeedback();
        }

        public void SetClickable(bool value)
        {
            _isClickable = value;
        }

        private void PlayFeedback()
        {
            if (_clickFeedback == null)
            {
                return;
            }

            _clickFeedback.PlayFeedback(transform.position);
        }
    }
}
