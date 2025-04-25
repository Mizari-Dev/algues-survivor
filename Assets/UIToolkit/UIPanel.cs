using UnityEngine;

public class UIPanel : UIView
{
    [SerializeField] private UIState _showUiState;
    [SerializeField] private UIState _hideUiState;
    [SerializeField] private bool _hideOnStart;

    protected override void LateInit()
    {
        if (_hideOnStart)
            HideInstant();
    }

    public void Show()
    {
        if (_showUiState != null)
            _showUiState.DoBehaviour(this);
    }
    public void Hide()
    {
        if (_hideUiState != null)
            _hideUiState.DoBehaviour(this);
    }
    public void ShowInstant()
    {
        if (_showUiState != null)
            _showUiState.DoBehaviourInstant(this);
    }
    public void HideInstant()
    {
        if (_hideUiState != null)
            _hideUiState.DoBehaviourInstant(this);
    }
}
