using UnityEngine;

public class MenuManager : UIPanelManager
{
    [SerializeField] private UIPanelController _credits;
    [SerializeField] private UIPanelController _tuto;

    public void Play()
    {
        _tuto.Show();
        Hide();
    }

    public void Credit()
    {
        _credits.Show();
        Hide();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
