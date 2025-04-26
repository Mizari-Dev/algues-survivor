using TMPro;
using UnityEngine;
using System.Collections;

public class DancingFont : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TMP_FontAsset _first;
    [SerializeField] private TMP_FontAsset _second;
    [SerializeField] private float _speed;
    private bool _state;
    private void Awake()
    {
        StartCoroutine(DanceInternal());
    }

    protected void Dance(bool state)
    {
        if (state)
            _text.font = _first;
        else
            _text.font = _second;
    }
    private IEnumerator DanceInternal()
    {
        while (true)
        {
            _state = !_state;
            Dance(_state);
            yield return new WaitForSeconds(_speed);
        }
    }
}
