using UnityEngine;
using DG.Tweening;

public class Scale : UIBehaviour
{
    [SerializeField] private Vector2 scale;

    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _animDuration = .2f;

    protected override void DoFromCurrent(UIView view)
    {
        Vector2 finalScale = (Vector2)view.Rt.localScale + scale;
        view.Rt.DOScale(finalScale, _animDuration).SetEase(_animCurve);
    }

    protected override void DoFromCurrentInstant(UIView view)
    {
        Vector2 finalScale = (Vector2)view.Rt.localScale + scale;
        view.Rt.localScale = finalScale;
    }

    protected override void DoFromStart(UIView view)
    {
        Vector2 finalScale = view.OriginalineScale + scale;
        view.Rt.DOScale(finalScale, _animDuration).SetEase(_animCurve);
    }

    protected override void DoFromStartInstant(UIView view)
    {
        Vector2 finalScale = view.OriginalineScale + scale;
        view.Rt.localScale = finalScale;
    }

    protected override void DoToCustom(UIView view)
    {
        view.Rt.DOScale(scale, _animDuration).SetEase(_animCurve);
    }

    protected override void DoToCustomInstant(UIView view)
    {
        view.Rt.localScale = scale;
    }
}
