using System;
using Cysharp.Threading.Tasks;
using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
using _01.Scripts.Outgame.Account.Repo;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Manager
{
    public class AccountManager : MonoBehaviour, IAccountService, ILoginStateProvider
    {
        public static AccountManager Instance { get; private set; }

        private IAccountRepository _repository;
        private Domain.Account _currentAccount;

        public bool IsLoggedIn => _currentAccount != null;
        public string CurrentAccountId => _currentAccount?.Id ?? string.Empty;

        public event Action<string> OnLoggedIn;
        public event Action OnLoggedOut;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _repository = new FirebaseAccountRepository();
        }

        public async UniTask<AccountResult> TryLogin(string id, string password)
        {
            AccountResult idResult = AccountValidator.ValidateId(id);
            if (!idResult.Success)
            {
                return idResult;
            }

            AccountResult passwordResult = AccountValidator.ValidatePassword(password);
            if (!passwordResult.Success)
            {
                return passwordResult;
            }

            AccountResult result = await _repository.Login(id, password);
            if (!result.Success)
            {
                return result;
            }

            string accountId = result.UserId ?? id;
            _currentAccount = new Domain.Account(accountId, password);
            OnLoggedIn?.Invoke(accountId);

            return AccountResult.Ok(accountId);
        }

        public async UniTask<AccountResult> TryRegister(string id, string password, string passwordConfirm)
        {
            AccountResult idResult = AccountValidator.ValidateId(id);
            if (!idResult.Success)
            {
                return idResult;
            }

            AccountResult passwordResult = AccountValidator.ValidatePassword(password);
            if (!passwordResult.Success)
            {
                return passwordResult;
            }

            AccountResult confirmResult = AccountValidator.ValidatePasswordConfirm(password, passwordConfirm);
            if (!confirmResult.Success)
            {
                return confirmResult;
            }

            AccountResult result = await _repository.Register(id, password);
            return result;
        }

        public void Logout()
        {
            _repository.Logout();
            _currentAccount = null;
            OnLoggedOut?.Invoke();
        }
    }
}