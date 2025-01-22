using TMPro;
using UnityEngine;

public class LoginFieldKeeper : MonoBehaviour
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI errorText;
    
    public void ButtonPressed()
    {
        string email = emailField.text;
        string password = passwordField.text;

        LoginManager.Instance.RegisterNewUser(email, password);
    }
}
