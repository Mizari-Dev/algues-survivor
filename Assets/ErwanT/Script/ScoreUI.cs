using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _point;

    public void Populate(PlayerScore score)
    {
        _name.text = score._name;
        _point.text = score._score.ToString();
    }
}
