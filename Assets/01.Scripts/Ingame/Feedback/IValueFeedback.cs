using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Ingame.Feedback
{
    public interface IValueFeedback
    {
        void Play(Vector3 position, BigNumber value);
        void Stop();
    }
}