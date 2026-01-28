using _01.Scripts.Core.Utils;
using _01.Scripts.Ingame.Click;

namespace _01.Scripts.Interfaces
{
    public interface IDamageable
    {
        public BigNumber CurrentHealth { get; }
        public BigNumber MaxHealth { get; }
        public bool IsDead { get; }

        void TakeDamage(ClickInfo clickInfo);
    }
}