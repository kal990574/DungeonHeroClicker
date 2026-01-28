using _01.Scripts.Core.Utils;

namespace _01.Scripts.Interfaces
{
    public interface ICurrencyProvider
    {
        public BigNumber GoldAmount { get; }

        void DropCurrency();
    }
}