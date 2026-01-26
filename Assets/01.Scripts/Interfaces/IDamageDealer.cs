namespace _01.Scripts.Interfaces
{
    public interface IDamageDealer
    {
        public float Damage { get; set; }
        void DealDamage(IDamageable target);
    }
}