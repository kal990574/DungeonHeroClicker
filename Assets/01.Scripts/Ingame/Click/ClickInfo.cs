using _01.Scripts.Core.Utils;
using UnityEngine;

namespace _01.Scripts.Ingame.Click
{
    public struct ClickInfo
    {
        public BigNumber Damage { get; }
        public EClickType ClickType { get; }
        public Vector3 Position { get; }
        public bool IsCritical { get; }

        public ClickInfo(BigNumber damage, EClickType clickType, Vector3 position, bool isCritical = false)
        {
            Damage = damage;
            ClickType = clickType;
            Position = position;
            IsCritical = isCritical;
        }
    }
}