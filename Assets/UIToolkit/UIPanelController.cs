using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private List<UIPanel> _panels;

    public void Show()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].Show();
    }
    public void Hide()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].Hide();
    }
    public void ShowInstant()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].ShowInstant();
    }
    public void HideInstant()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].HideInstant();
    }

}
