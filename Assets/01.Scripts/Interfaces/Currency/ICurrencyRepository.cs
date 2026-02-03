using System;
using _01.Scripts.Outgame.Currency.Repo;

namespace _01.Scripts.Interfaces.Currency
{
    public interface ICurrencyRepository
    {
        void Save(CurrencySaveData data);
        CurrencySaveData Load();
        event Action OnSaveRequested;
    }
}