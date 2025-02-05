using UnityEngine;


[CreateAssetMenu(fileName = "IDComponent", menuName = "Scriptable Objects/IDComponent")]
public class IDComponent : ScriptableObject
{
    public string typeOfComponent;
    public Sprite[] variations;
}
