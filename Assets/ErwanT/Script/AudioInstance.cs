using UnityEngine;
using System.Collections;
public class AudioInstance : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    private AudioReference _reference;
    private SoundManager _manager;
    public AudioReference Reference => _reference;

    public void Populate(SoundManager manager, AudioReference reference)
    {
        _manager = manager;
        _reference = reference;
        StartCoroutine(InternalPopulate());
    }

    private IEnumerator InternalPopulate()
    {
        _source.clip = _reference.Base;
        _source.Play();
        yield return new WaitUntil(() => !_source.isPlaying);
        if(_reference.Loop == null)
        {
            _manager.RemoveSound(this);
            yield break;
        }
        _source.clip = _reference.Loop;
        _source.loop = true;
        _source.Play();
    }
}
