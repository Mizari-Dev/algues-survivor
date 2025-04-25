using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanelController : MonoBehaviour
{
    [SerializeField] private List<UIPanel> _panels;
    [SerializeField] private GameObject _selectOnShow;

    public void Show()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].Show();
        EventSystem.current.SetSelectedGameObject(_selectOnShow);
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
        EventSystem.current.SetSelectedGameObject(_selectOnShow);
    }
    public void HideInstant()
    {
        for (int i = 0; i < _panels.Count; i++)
            _panels[i].HideInstant();
    }

}
