using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    FirebaseAuth auth;
    string key;

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
    }

    private async void Start()
    {
        InternetChecker.IsInternetAvailable(() => { });
        await Init();

    }
    public async Task Init()
    {
        Debug.Log("Init Runs");
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError("Firebase dependencies are not available: " + dependencyStatus);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        if (auth == null)
        {
            Debug.LogError("FirebaseAuth initialization failed.");
            return;
        }

        Debug.Log("Firebase Auth initialized successfully.");

        key = await GetKey();

        await CheckCredentials();
    }




    public void RegisterNewUser(string email, string password)
    {
        if (auth == null)
        {
            Debug.LogError("FirebaseAuth is not initialized.");
            return;
        }

        Debug.Log("Attempting to register new user with email: " + email);

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = errorCode switch
                {
                    AuthError.MissingEmail => "Enter your E-mail address",
                    AuthError.MissingPassword => "Enter your password",
                    AuthError.WrongPassword => "Wrong Password",
                    AuthError.UserDisabled => "User Disabled",
                    _ => "Register failed!"
                };

                Debug.LogError(firebaseEx);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;

                SaveCredentials(email, password);
                SaveDataManager.Instance.SavePlayer();
                Invoke(nameof(SignIn), 0.5f);
            }
        });
    }




    private async Task SignIn()
    {
        string[] credentials = GetCredentials();
        string email = credentials[0];
        string password = credentials[1];

        try
        {
            AuthResult result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = result.User;
            SaveCredentials(email, password);

            RPSMatchMaking.Instance.userId = auth.CurrentUser.UserId;
            SaveDataManager.Instance.LoadPlayer();
        }
        catch (FirebaseException firebaseEx)
        {
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = errorCode switch
            {
                AuthError.MissingEmail => "Enter your E-mail address",
                AuthError.MissingPassword => "Enter your password",
                AuthError.WrongPassword => "Wrong Password",
                AuthError.UserDisabled => "User Disabled",
                _ => "Something went wrong!"
            };
            Debug.Log(firebaseEx);
            FindAnyObjectByType<LoginFieldKeeper>().errorText.text = message;
        }
    }


    private void SaveCredentials(string email, string password)
    {
        var encryptedEmail = Cryptography.Encrypt(email, key);
        var encryptedPassword = Cryptography.Encrypt(password, key);

        PlayerPrefs.SetString("email", encryptedEmail);
        PlayerPrefs.SetString("password", encryptedPassword);
    }


    private static async Task<string> GetKey()
    {
        var snapshot = await FirebaseDatabase.DefaultInstance.RootReference.Child("key").GetValueAsync();

        return snapshot.Exists ? snapshot.Value.ToString().Trim('"') : "";
    }


    private async Task CheckCredentials()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("email")) &&
            !string.IsNullOrEmpty(PlayerPrefs.GetString("password")))
        {
            await SignIn();
        }
    }


    private string[] GetCredentials()
    {
        string email = Cryptography.Decrypt(PlayerPrefs.GetString("email"), key);
        string password = Cryptography.Decrypt(PlayerPrefs.GetString("password"), key);

        return new string[] { email, password };
    }


    public void SignOut()
    {
        auth.SignOut();
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");

        SceneController.Instance.GoToScene("Login");
    }
}