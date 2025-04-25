using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIState", menuName = "Data/UI/UIState")]
public class UIState : ScriptableObject
{
    [SerializeField, SerializeReference] private List<UIBehaviour> _behaviours = new List<UIBehaviour>();
    public void DoBehaviour(UIView view)
    {
        for (int i = 0; i < _behaviours.Count; i++)
        {
            UIBehaviour behaviour = _behaviours[i];
            behaviour.DoBehaviour(view);
        }
    }
    public void DoBehaviourInstant(UIView view)
    {
        for (int i = 0; i < _behaviours.Count; i++)
        {
            UIBehaviour behaviour = _behaviours[i];
            behaviour.DoBehaviourInstant(view);
        }
    }
}
