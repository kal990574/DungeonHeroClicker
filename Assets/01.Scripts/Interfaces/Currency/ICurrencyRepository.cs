using Cysharp.Threading.Tasks;
using _01.Scripts.Outgame.Currency.Repo;

namespace _01.Scripts.Interfaces.Currency
{
    public interface ICurrencyRepository
    {
        UniTask Save(CurrencySaveData data);
        UniTask<CurrencySaveData> Load();
    }
}