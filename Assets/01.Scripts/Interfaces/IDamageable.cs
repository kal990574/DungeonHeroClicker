using _01.Scripts.Ingame.Click;

namespace _01.Scripts.Interfaces
{
    public interface IDamageable
    {
        public float CurrentHealth { get; }
        public float MaxHealth { get; }
        public bool IsDead { get; }
        
        void TakeDamage(ClickInfo clickInfo);
    }
}