namespace _01.Scripts.Interfaces
{
    public interface ICurrencyProvider
    {
        public int GoldAmount { get; }
        
        void DropCurrency();
    }
}