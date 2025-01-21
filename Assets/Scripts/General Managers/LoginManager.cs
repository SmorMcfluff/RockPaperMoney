using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine.Timeline;

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
        }
        DontDestroyOnLoad(gameObject);
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
        });

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SaveDataManager.Instance.LoadPlayer();
            SceneController.Instance.GoToScene("MainMenu");
        }
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
                FindAnyObjectByType<LoginFieldKeeper>().errorText.text = "User Registered!";
                var saveData = SaveDataManager.Instance.localPlayerData;
                SaveDataManager.Instance.SavePlayer();
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
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                SaveDataManager.Instance.LoadPlayer();
                SceneController.Instance.GoToScene("MainMenu");
            }
        });
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneController.Instance.GoToScene("Login");
    }
}
