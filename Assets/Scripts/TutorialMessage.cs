using UnityEngine;

[CreateAssetMenu(fileName = "TutorialMessage", menuName = "Scriptable Objects/TutorialMessage")]
public class TutorialMessage : ScriptableObject
{
    [TextArea]
    public string message;
}
