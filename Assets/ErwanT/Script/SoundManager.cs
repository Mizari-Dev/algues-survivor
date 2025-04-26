using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioInstance _prefab;
    [SerializeField] private AudioReference _referenceOnStart;
    private List<AudioInstance> _runningInstance = new List<AudioInstance>();
    private float _danceRythms;
    private bool _danceState;
    private bool _isDancing;
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

    private void Dance(float value)
    {
        _danceRythms = value;
        if (_isDancing)
            return;
        _isDancing = true;
        StartCoroutine(DanceInternal());
    }

    private IEnumerator DanceInternal()
    {
        while (_isDancing && _danceRythms > 0)
        {
            _danceState = !_danceState;
            Events.DoDanceState(_danceState);
            yield return new WaitForSeconds(_danceRythms);
        }
    }

    public void PlaySound(AudioReference reference)
    {
        if (reference.Single)
            if (_runningInstance.Exists(x => x.Reference == reference))
                return;
        if (reference.DanceRythms > 0)
            Dance(reference.DanceRythms);
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
        _danceRythms = 0;
    }
}
