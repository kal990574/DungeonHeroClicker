using _01.Scripts.Core.Utils;

namespace _01.Scripts.Interfaces
{
    public interface IDamageDealer
    {
        public BigNumber Damage { get; set; }
        void DealDamage(IDamageable target);
    }
}