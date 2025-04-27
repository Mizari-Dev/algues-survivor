using UnityEngine;

public class ActionUI : UIPanelManager
{
    [SerializeField] private AudioReference _onPress;
    [SerializeField] private ScriptableKeyBind _yellowKeyBind;
    [SerializeField] private ScriptableKeyBind _blueKeyBind;
    [SerializeField] private ParticleSystem _yellowParticle;
    [SerializeField] private ParticleSystem _blueParticle;

    private void Awake()
    {
        if (_yellowKeyBind == null)
            return;
        if (_blueKeyBind == null)
            return;
        _yellowKeyBind._onStart += YellowShowInternal;
        _blueKeyBind._onStart += BlueShowInternal;
    }

    private void OnDestroy()
    {
        if (_yellowKeyBind == null)
            return;
        if (_blueKeyBind == null)
            return;
        _yellowKeyBind._onStart -= YellowShowInternal;
        _blueKeyBind._onStart -= BlueShowInternal;
    }

    private void YellowShowInternal()
    {
        SoundManager.Instance.PlaySound(_onPress);
        _yellowParticle.Play();
    }
    private void BlueShowInternal()
    {
        SoundManager.Instance.PlaySound(_onPress);
        _blueParticle.Play();
    }
}
