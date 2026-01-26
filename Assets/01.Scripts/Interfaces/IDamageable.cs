namespace _01.Scripts.Interfaces
{
    public interface IDamageable
    {
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public bool IsDead { get; set; }
        
        void TakeDamage(float damage);
    }
}