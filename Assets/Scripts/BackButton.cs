using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    [SerializeField] string targetScene = "MainMenu";

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(GoToTargetScene);
    }

    private void GoToTargetScene()
    {
        SceneController.Instance.GoToScene(targetScene);
    }
}
