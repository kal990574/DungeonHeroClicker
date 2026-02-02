using System;

namespace _01.Scripts.Interfaces.Account
{
    public interface ILoginStateProvider
    {
        bool IsLoggedIn { get; }
        string CurrentAccountId { get; }
        event Action<string> OnLoggedIn;
        event Action OnLoggedOut;
    }
}