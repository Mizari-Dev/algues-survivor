using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : UIPanelManager
{
    [SerializeField] private UIPanelController _tuto;

    public void Tuto()
    {
        _tuto.Show();
        Hide();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
