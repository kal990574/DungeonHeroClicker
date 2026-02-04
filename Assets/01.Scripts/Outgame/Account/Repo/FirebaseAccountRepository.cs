using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
using UnityEngine;
using AuthResult = _01.Scripts.Outgame.Account.Domain.AuthResult;

namespace _01.Scripts.Outgame.Account.Repo
{
    public class FirebaseAccountRepository : IFirebaseAccountRepository
    {
        private readonly FirebaseAuth _auth;

        public FirebaseAccountRepository(FirebaseAuth auth)
        {
            _auth = auth;
        }

        public bool IsLoggedIn => _auth.CurrentUser != null;

        public string CurrentUserId => _auth.CurrentUser?.UserId ?? string.Empty;

        public async UniTask<AuthResult> RegisterAsync(string email, string password)
        {
            try
            {
                await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
                return AuthResult.Ok();
            }
            catch (FirebaseException e)
            {
                Debug.LogError("Register failed: " + e.Message);
                return AuthResult.Fail(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Register failed: " + e.Message);
                return AuthResult.Fail(e.Message);
            }
        }

        public async UniTask<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
                return AuthResult.Ok();
            }
            catch (FirebaseException e)
            {
                Debug.LogError("Login failed: " + e.Message);
                return AuthResult.Fail(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Login failed: " + e.Message);
                return AuthResult.Fail(e.Message);
            }
        }

        public void Logout()
        {
            _auth.SignOut();
        }
    }
}