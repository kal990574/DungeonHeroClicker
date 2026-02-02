using System;
using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
using _01.Scripts.Outgame.Account.Repo;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Manager
{
    public class AccountManager : MonoBehaviour, IAccountService, ILoginStateProvider
    {
        public static AccountManager Instance { get; private set; }

        [SerializeField] private AccountRepositoryBridge _repositoryBridge;

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

            _repository = _repositoryBridge.Repository;
        }

        public AuthResult TryLogin(string id, string password)
        {
            AuthResult idResult = AccountValidator.ValidateId(id);
            if (!idResult.Success)
            {
                return idResult;
            }

            AuthResult passwordResult = AccountValidator.ValidatePassword(password);
            if (!passwordResult.Success)
            {
                return passwordResult;
            }

            if (!_repository.Exists(id))
            {
                return AuthResult.Fail("Invalid ID or password.");
            }

            AccountSaveData savedData = _repository.Load(id);
            if (savedData.Password != password)
            {
                return AuthResult.Fail("Invalid ID or password.");
            }

            _currentAccount = new Domain.Account(id, password);
            OnLoggedIn?.Invoke(id);

            return AuthResult.Ok();
        }

        public AuthResult TryRegister(string id, string password, string passwordConfirm)
        {
            AuthResult idResult = AccountValidator.ValidateId(id);
            if (!idResult.Success)
            {
                return idResult;
            }

            AuthResult passwordResult = AccountValidator.ValidatePassword(password);
            if (!passwordResult.Success)
            {
                return passwordResult;
            }

            AuthResult confirmResult = AccountValidator.ValidatePasswordConfirm(password, passwordConfirm);
            if (!confirmResult.Success)
            {
                return confirmResult;
            }

            if (_repository.Exists(id))
            {
                return AuthResult.Fail("This ID is already taken.");
            }

            var saveData = new AccountSaveData
            {
                Id = id,
                Password = password
            };

            _repository.Save(saveData);

            return AuthResult.Ok();
        }

        public void Logout()
        {
            _currentAccount = null;
            OnLoggedOut?.Invoke();
        }
    }
}