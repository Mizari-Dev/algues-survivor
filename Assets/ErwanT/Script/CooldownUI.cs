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
        _text.text = value.ToString();
    }
}
