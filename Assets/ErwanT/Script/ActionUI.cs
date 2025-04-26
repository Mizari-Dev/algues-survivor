using UnityEngine;

public class ActionUI : UIPanelManager
{
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
        _yellowKeyBind._onCancel += Hide;
        _blueKeyBind._onStart += BlueShowInternal;
        _blueKeyBind._onCancel += Hide;
    }

    private void OnDestroy()
    {
        if (_yellowKeyBind == null)
            return;
        if (_blueKeyBind == null)
            return;
        _yellowKeyBind._onStart -= YellowShowInternal;
        _yellowKeyBind._onCancel -= Hide;
        _blueKeyBind._onStart -= BlueShowInternal;
        _blueKeyBind._onCancel -= Hide;
    }

    private void YellowShowInternal()
    {
        Show();
        _yellowParticle.Play();
    }
    private void BlueShowInternal()
    {
        Show();
        _blueParticle.Play();
    }
}
