using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public interface IFeedback
    {
        void Play(Vector3 position);
        void Stop();
    }
}