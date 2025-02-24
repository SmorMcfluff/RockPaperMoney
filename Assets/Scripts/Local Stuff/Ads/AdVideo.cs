using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "AdVideo", menuName = "Scriptable Objects/AdVideo")]
public class AdVideo : ScriptableObject
{
    public VideoClip clip;

    [FormerlySerializedAs("link")]
    public string url;
}
