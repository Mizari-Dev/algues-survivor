using UnityEngine;
using DG.Tweening;
using System;

[Serializable,AddTypeMenu("Fade")]
public class Fade : UIBehaviour
{
    [SerializeField] private float _alpha;
    [SerializeField] private bool _blockRaycast;
    [SerializeField] private bool _interactable;

    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _animDuration = .2f;

    protected override void DoFromCurrent(UIView view)
    {
        view.Cg.DOFade(view.Cg.alpha + _alpha, _animDuration).SetEase(_animCurve);
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }

    protected override void DoFromCurrentInstant(UIView view)
    {
        view.Cg.alpha = view.Cg.alpha + _alpha;
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }

    protected override void DoFromStart(UIView view)
    {
        view.Cg.DOFade(view.OrigineAlpha + _alpha, _animDuration).SetEase(_animCurve);
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }

    protected override void DoFromStartInstant(UIView view)
    {
        view.Cg.alpha = view.OrigineAlpha + _alpha;
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }

    protected override void DoToCustom(UIView view)
    {
        view.Cg.DOFade(_alpha, _animDuration).SetEase(_animCurve);
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }

    protected override void DoToCustomInstant(UIView view)
    {
        view.Cg.alpha = _alpha;
        view.Cg.blocksRaycasts = _blockRaycast;
        view.Cg.interactable = _interactable;
    }
}
