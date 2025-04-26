using UnityEngine;

public class MenuManager : UIPanelManager
{
    [SerializeField] private UIPanelController _tuto;

    public void Play()
    {
        _tuto.Show();
        Hide();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
