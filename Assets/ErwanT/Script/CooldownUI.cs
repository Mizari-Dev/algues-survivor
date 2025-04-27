using TMPro;
using UnityEngine;

public class CooldownUI : UIPanelManager
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private PowerType _power;
    private int _internalValue;

    private void Awake()
    {
        Events._onSetCooldown += SetCooldown;
    }

    private void OnDestroy()
    {
        Events._onSetCooldown -= SetCooldown;
    }

    private void SetCooldown(PowerType type, int value)
    {
        if (type != _power)
            return;
        if (value <= 0)
            Hide();
        else
            Show();
        _text.text = value.ToString();
    }
}
