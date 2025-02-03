using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    FirebaseAuth auth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        GetKey();
    }



    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            auth = FirebaseAuth.DefaultInstance;
            CheckCredentials();
        });
    }


    public void RegisterNewUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            string message = "";
            if (task.Exception != null)
            {
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.EmailAlreadyInUse:
                        message = "Attempting Log In";
                        SignIn(email, password);
                        break;
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    default:
                        Debug.LogWarning(task.Exception);
                        break;
                }
                FindAnyObjectByType<LoginFieldKeeper>().errorText.text = message;
                Debug.Log(message);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;

                SaveCredentials(email, password);

                FindAnyObjectByType<LoginFieldKeeper>().errorText.text = "User Registered!";
                var saveData = SaveDataManager.Instance.localPlayerData;

                SaveDataManager.Instance.SavePlayer();
                Invoke(nameof(SignIn), 1.5f);
            }
        });
    }


    private void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);

                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Sign In Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Enter your E-mail address";
                        break;
                    case AuthError.MissingPassword:
                        message = "Enter your password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.UserDisabled:
                        message = "User Disabled";
                        break;
                    case AuthError.Failure:
                        message = "Something went wrong!";
                        break;
                    default:
                        Debug.LogWarning(task.Exception);
                        break;
                }
                Debug.Log(message);

                FindAnyObjectByType<LoginFieldKeeper>().errorText.text = message;
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                SaveCredentials(email, password);

                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                SaveDataManager.Instance.LoadPlayer();
                SceneController.Instance.GoToScene("MainMenu");
            }
        });
    }


    private void SaveCredentials(string email, string password)
    {
        var key = GetKey();

        var encryptedEmail = Cryptography.Encrypt(email, key);
        var encryptedPassword = Cryptography.Encrypt(password, key);

        PlayerPrefs.SetString("email", encryptedEmail);
        PlayerPrefs.SetString("password", encryptedPassword);
    }


    private static string GetKey()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("key").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            string key = task.Result.GetRawJsonValue();
            return key.Trim('"');
        });
        return "";
    }


    private void CheckCredentials()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("email")) && !string.IsNullOrEmpty(PlayerPrefs.GetString("password")))
        {
            string[] credentials = GetCredentials();
            RegisterNewUser(credentials[0], credentials[1]);
        }
    }


    private string[] GetCredentials()
    {
        var key = GetKey();

        string email = Cryptography.Decrypt(PlayerPrefs.GetString("email"), key);
        string password = Cryptography.Decrypt(PlayerPrefs.GetString("password"), key);

        string[] credentials = { email, password };
        return credentials;
    }


    public void SignOut()
    {
        auth.SignOut();
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");

        SceneController.Instance.GoToScene("Login");
    }
}