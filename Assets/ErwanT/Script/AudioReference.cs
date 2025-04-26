using UnityEngine;

[CreateAssetMenu(fileName = "AudioReference", menuName = "Data/AudioReference")]
public class AudioReference : ScriptableObject
{
    [SerializeField] private AudioClip _base;
    [SerializeField] private AudioClip _loop;
    [SerializeField] private bool _single;
    public AudioClip Base => _base;
    public AudioClip Loop => _loop;
    public bool Single => _single;
}
