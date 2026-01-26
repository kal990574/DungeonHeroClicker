using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public interface IValueFeedback
    {
        void Play(Vector3 position, float value);
        void Stop();
    }
}