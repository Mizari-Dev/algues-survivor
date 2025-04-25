using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(UISelectableController))]
public class CustomButton : Button
{
    [SerializeField] private UISelectableController _view;
    [SerializeField] private RectTransform _rect;
    [SerializeField, SerializeReference] private List<UIElement> _UIElements = new List<UIElement>();

    protected override void Start()
    {
        base.Start();
        _rect = (RectTransform)transform;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Clear();
    }

    public void Clear()
    {
        onClick = null;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        if (_view == null)
            return;
        SelectionStateType convertedState = (SelectionStateType)state;
        _view.SetSelectable(convertedState);
    }

}
