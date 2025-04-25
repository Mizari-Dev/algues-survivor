using UnityEngine;

public class SetActive : UIBehaviour
{
    [SerializeField] private bool _state;

    protected override void DoFromCurrent(UIView view)
    {
        view.gameObject.SetActive(_state);
    }

    protected override void DoFromCurrentInstant(UIView view)
    {
        view.gameObject.SetActive(_state);
    }

    protected override void DoFromStart(UIView view)
    {
        view.gameObject.SetActive(_state);
    }

    protected override void DoFromStartInstant(UIView view)
    {
        view.gameObject.SetActive(_state);
    }

    protected override void DoToCustom(UIView view)
    {
        view.gameObject.SetActive(_state);
    }

    protected override void DoToCustomInstant(UIView view)
    {
        view.gameObject.SetActive(_state);
    }
}
