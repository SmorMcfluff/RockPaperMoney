using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] string targetScene = "MainMenu";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { SceneController.Instance.GoToScene(targetScene); });
    }
}
