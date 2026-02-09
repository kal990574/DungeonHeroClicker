#if !UNITY_WEBGL
using System;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace _01.Scripts.Core
{
    public class FirebaseInitializer : MonoBehaviour
    {
        public static FirebaseInitializer Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitFirebase().Forget();
        }

        private async UniTask InitFirebase()
        {
            try
            {
                DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

                if (status == DependencyStatus.Available)
                {
                    Debug.Log("Firebase init success.");
                }
                else
                {
                    Debug.LogError("Firebase init failed: " + status);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Firebase init failed: " + e.Message);
            }
        }
    }
}
#endif