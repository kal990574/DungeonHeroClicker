using _01.Scripts.Interfaces.Account;
using _01.Scripts.Outgame.Account.Domain;
using _01.Scripts.Outgame.Account.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01.Scripts.Core.Login
{
    public class LoginScene : MonoBehaviour
    {
        private enum SceneMode
        {
            Login,
            Register
        }

        private SceneMode _mode = SceneMode.Login;

        [Header("Account")]
        [SerializeField] private AccountManager _accountManager;

        [Header("UI - Buttons")]
        [SerializeField] private Button _gotoRegisterButton;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _gotoLoginButton;
        [SerializeField] private Button _registerButton;

        [Header("UI - Input Fields")]
        [SerializeField] private TMP_InputField _idInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _passwordConfirmInputField;

        [Header("UI - Message")]
        [SerializeField] private GameObject _passwordConfirmObject;
        [SerializeField] private TextMeshProUGUI _messageTextUI;

        private IAccountService _accountService;

        private void Awake()
        {
            _accountService = _accountManager;
        }

        private void Start()
        {
            AddButtonEvents();
            Refresh();
        }

        private void AddButtonEvents()
        {
            _gotoRegisterButton.onClick.AddListener(GotoRegister);
            _loginButton.onClick.AddListener(Login);
            _gotoLoginButton.onClick.AddListener(GotoLogin);
            _registerButton.onClick.AddListener(Register);
        }

        private void Refresh()
        {
            _passwordConfirmObject.SetActive(_mode == SceneMode.Register);
            _gotoRegisterButton.gameObject.SetActive(_mode == SceneMode.Login);
            _loginButton.gameObject.SetActive(_mode == SceneMode.Login);
            _gotoLoginButton.gameObject.SetActive(_mode == SceneMode.Register);
            _registerButton.gameObject.SetActive(_mode == SceneMode.Register);
            _messageTextUI.text = string.Empty;
        }

        private void Login()
        {
            AuthResult result = _accountService.TryLogin(
                _idInputField.text,
                _passwordInputField.text);

            if (!result.Success)
            {
                _messageTextUI.text = result.ErrorMessage;
                return;
            }

            SceneManager.LoadScene("GameScene");
        }

        private void Register()
        {
            AuthResult result = _accountService.TryRegister(
                _idInputField.text,
                _passwordInputField.text,
                _passwordConfirmInputField.text);

            if (!result.Success)
            {
                _messageTextUI.text = result.ErrorMessage;
                return;
            }

            GotoLogin();
            _messageTextUI.text = "Registration successful! Please log in.";
        }

        private void GotoLogin()
        {
            _mode = SceneMode.Login;
            Refresh();
        }

        private void GotoRegister()
        {
            _mode = SceneMode.Register;
            Refresh();
        }
    }
}