using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScriptableKeyBind", menuName = "SO/Other/KeyBind")]
public class ScriptableKeyBind : ScriptableObject
{
    [SerializeField] private InputAction _control;

    public delegate void OnCall();
    public event OnCall _onStart;
    private void OnStart(InputAction.CallbackContext ctx) => _onStart?.Invoke();

    public event OnCall _onPerformed;
    private void OnPerformed(InputAction.CallbackContext ctx) => _onPerformed?.Invoke();

    public event OnCall _onCancel;
    private void OnCancel(InputAction.CallbackContext ctx) => _onCancel?.Invoke();

    public delegate void OnChangeBind(string bind);
    public event OnChangeBind _onChangeBinding;
    private void OnChangeBinding(string _newKey) => _onChangeBinding?.Invoke(_newKey);

    private void OnEnable()
    {
        _onStart = null;
        _onPerformed = null;
        _onCancel = null;
            _control.started += OnStart;
            _control.performed += OnPerformed;
            _control.canceled += OnCancel;
        RefreshBind();
    }

    public string GetKeyText()
    {
        string text = GetBindingPath();
        return text.Split("/").Last().ToUpper();
    }

    private void RefreshBind()
    {
        _control.Disable();
        _control.Enable();
        OnChangeBinding(GetKeyText());
    }

    public string GetBindingPath()
    {
        return GetFirstBinding();
    }

    private string GetFirstBinding()
    {
        if (_control.bindings[0] != null)
            return _control.GetBindingDisplayString();
        else
            return "";
    }
}
