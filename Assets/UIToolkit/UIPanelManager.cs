using UnityEngine;

[RequireComponent(typeof(UIPanelController))]
public abstract class UIPanelManager : MonoBehaviour
{
    [SerializeField] protected UIPanelController _panel;
    public virtual void Show() => _panel.Show();
    public virtual void Hide() => _panel.Hide();
    public virtual void ShowInstant() => _panel.ShowInstant();
    public virtual void HideInstant() => _panel.HideInstant();
}
