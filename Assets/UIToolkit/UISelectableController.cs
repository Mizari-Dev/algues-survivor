using System.Collections.Generic;
using UnityEngine;

public class UISelectableController : MonoBehaviour
{
    [SerializeField] private List<UISelectable> _selectable = new List<UISelectable>();
    public void SetSelectable(SelectionStateType state)
    {
        for (int i = 0; i < _selectable.Count; i++)
            _selectable[i].SetSelectable(state);
    }
}
