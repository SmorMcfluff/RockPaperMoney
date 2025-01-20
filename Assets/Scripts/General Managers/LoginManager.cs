using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    FirebaseAuth auth;

    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TextMeshProUGUI errorText;

    public void Start()
    {
        passwordField.contentType = TMP_InputField.ContentType.Password;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });
    }


    public void ButtonPressed()
    {
        string email = emailField.text;
        string password = passwordField.text;

        Debug.Log(email + ", " + password);

        RegisterNewUser(email, password);
    }


    private void RegisterNewUser(string email, string password)
    {
        Debug.Log("Starting Registration");
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
                errorText.text = message;
                Debug.Log(message);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                errorText.text = "User Registered!";
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

                errorText.text = message;
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                SaveDataManager.Instance.LoadPlayer();
                SceneManager.LoadScene("IdleGameScene");
            }
        });
    }
}
