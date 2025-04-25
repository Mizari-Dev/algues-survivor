using DG.Tweening;
using UnityEngine;

public class Rotate : UIBehaviour
{
    [SerializeField] private Vector3 _rotation;

    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _animDuration = .2f;

    protected override void DoFromCurrent(UIView view)
    {
        Vector3 fRotation = new Vector3(view.Rt.rotation.x, view.Rt.rotation.y, view.Rt.rotation.z);
        fRotation += _rotation;
        view.Rt.DORotate(fRotation, _animDuration).SetEase(_animCurve);
    }

    protected override void DoFromCurrentInstant(UIView view)
    {
        Quaternion fRotation = view.Rt.rotation;
        fRotation.x += _rotation.x;
        fRotation.y += _rotation.y;
        fRotation.z += _rotation.z;
        view.Rt.rotation = fRotation;
    }

    protected override void DoFromStart(UIView view)
    {
        Vector3 fRotation = view.OrigineRotation;
        fRotation += _rotation;
        view.Rt.DORotate(fRotation, _animDuration).SetEase(_animCurve);
    }

    protected override void DoFromStartInstant(UIView view)
    {
        Quaternion fRotation = view.Rt.rotation;
        fRotation.x = view.OrigineRotation.x + _rotation.x;
        fRotation.y = view.OrigineRotation.y + _rotation.y;
        fRotation.z = view.OrigineRotation.z + _rotation.z;
        view.Rt.rotation = fRotation;
    }

    protected override void DoToCustom(UIView view)
    {
        Vector3 fRotation = _rotation;
        view.Rt.DORotate(fRotation, _animDuration).SetEase(_animCurve);
    }

    protected override void DoToCustomInstant(UIView view)
    {
        Quaternion fRotation = Quaternion.Euler(_rotation);
        view.Rt.rotation = fRotation;
    }
}
