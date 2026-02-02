using _01.Scripts.Interfaces;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public class ClickTarget : MonoBehaviour, IClickable
    {
        private bool _isClickable = true;

        public bool IsClickable => _isClickable;

        public void OnClick()
        {
            if (!_isClickable)
            {
                return;
            }
        }

        public void SetClickable(bool value)
        {
            _isClickable = value;
        }
    }
}
