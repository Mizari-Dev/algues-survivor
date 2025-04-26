using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIState", menuName = "Data/UI/UIState")]
public class UIState : ScriptableObject
{
    [SerializeField, SerializeReference,SubclassSelector] private List<UIBehaviour> _behaviours = new List<UIBehaviour>();
    [SerializeField] private List<AudioReference> _sounds = new List<AudioReference>();
    public void DoBehaviour(UIView view)
    {
        for (int i = 0; i < _sounds.Count; i++)
            SoundManager.Instance.PlaySound(_sounds[i]);
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
