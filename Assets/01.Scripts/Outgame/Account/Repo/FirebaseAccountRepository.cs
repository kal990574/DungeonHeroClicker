#if !UNITY_WEBGL
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
using UnityEngine;

namespace _01.Scripts.Outgame.Account.Repo
{
    public class FirebaseAccountRepository : IAccountRepository
    {
        private readonly FirebaseAuth _auth;

        public FirebaseAccountRepository()
        {
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async UniTask<AccountResult> Register(string email, string password)
        {
            try
            {
                await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
                return AccountResult.Ok();
            }
            catch (FirebaseException e)
            {
                Debug.LogError("Register failed: " + e.Message);
                return AccountResult.Fail(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Register failed: " + e.Message);
                return AccountResult.Fail(e.Message);
            }
        }

        public async UniTask<AccountResult> Login(string email, string password)
        {
            try
            {
                await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
                return AccountResult.Ok(_auth.CurrentUser.UserId);
            }
            catch (FirebaseException e)
            {
                Debug.LogError("Login failed: " + e.Message);
                return AccountResult.Fail(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Login failed: " + e.Message);
                return AccountResult.Fail(e.Message);
            }
        }

        public void Logout()
        {
            _auth.SignOut();
        }
    }
}
#endif