using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

namespace _01.Scripts.Core
{
    public class FirebaseInitializer : MonoBehaviour
    {
        public static FirebaseInitializer Instance { get; private set; }

        public FirebaseFirestore Db { get; private set; }
        public FirebaseAuth Auth { get; private set; }
        public bool IsInitialized { get; private set; }

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

        public async UniTask InitAsync()
        {
            if (IsInitialized)
            {
                return;
            }

            DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (status != DependencyStatus.Available)
            {
                Debug.LogError("Firebase init failed: " + status);
                return;
            }

            Auth = FirebaseAuth.DefaultInstance;
            Db = FirebaseFirestore.DefaultInstance;
            IsInitialized = true;

            Debug.Log("Firebase init success.");
        }
    }
}