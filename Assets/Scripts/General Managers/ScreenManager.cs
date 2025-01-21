using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    void Awake()
    {
        Camera.main.aspect = 0.5625f;
        Application.targetFrameRate = 60;
    }
}
