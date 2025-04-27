using UnityEngine;
using UnityEngine.UI;

public class TimerUI : UIPanelManager
{
    [SerializeField] private Image _prefab;
    [SerializeField] private Transform _container;

    private void Awake()
    {
        Events._setTimer += SetTimer;
    }

    private void OnDestroy()
    {
        Events._setTimer -= SetTimer;
    }

    private void SetTimer(float timer)
    {
        Clear();
        for (int i = 0; i < timer; i++)
        {
            Image tick = Instantiate(_prefab, _container);
        }
    }

    private void Clear()
    {
        foreach (Transform T in _container)
            Destroy(T.gameObject);
    }
}
