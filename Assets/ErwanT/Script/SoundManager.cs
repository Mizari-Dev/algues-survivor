using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioInstance _prefab;
    [SerializeField] private AudioReference _referenceOnStart;
    private List<AudioInstance> _runningInstance = new List<AudioInstance>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        if (_referenceOnStart != null)
            PlaySound(_referenceOnStart);
    }

    public void PlaySound(AudioReference reference)
    {
        if (reference.Single)
            if (_runningInstance.Exists(x => x.Reference == reference))
                return;
        AudioInstance newInstance = Instantiate(_prefab, transform);
        newInstance.Populate(this,reference);
        _runningInstance.Add(newInstance);
    }

    public void RemoveSound(AudioReference reference)
    {
        AudioInstance instance = _runningInstance.Find(x => x.Reference == reference);
        if (instance == null)
            return;
        RemoveSound(instance);
    }
    public void RemoveSound(AudioInstance reference)
    {
        if (!_runningInstance.Contains(reference))
            return;
        _runningInstance.Remove(reference);
        Destroy(reference.gameObject);
    }

    public void ClearAllSound()
    {
        for (int i = 0; i < _runningInstance.Count; i++)
            RemoveSound(_runningInstance[i]);
    }
}
