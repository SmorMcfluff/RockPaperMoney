using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEditor.PackageManager;

public class LoginManager : MonoBehaviour
{
    FirebaseAuth auth;

    [SerializeField] TMP_InputField emailField;
    [SerializeField] TMP_InputField passwordField;

    public void Start()
    {
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

        if(!string.IsNullOrEmpty(email))
        {
            RegisterNewUser(email, password);
        }
    }


    private void RegisterNewUser(string email, string password)
    {
        Debug.Log("Starting Registration");
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User Registered: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                DataTest(newUser.UserId, "abc123");
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
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
            }
        });
    }

    private void DataTest(string userID, string data)
    {
        Debug.Log("Trying to write data...");
        var db = FirebaseDatabase.DefaultInstance;
        db.RootReference.Child("users").Child(userID).SetValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
                Debug.Log("DataTestWrite: Complete");
        });
    }
}
