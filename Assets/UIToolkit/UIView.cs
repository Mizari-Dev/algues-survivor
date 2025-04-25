using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIView : MonoBehaviour
{
    private bool _isShow;
    [SerializeField] private RectTransform _rt;
    [SerializeField] private CanvasGroup _cg;

    private Vector2 _originePosition;
    private Vector2 _origineScale;
    private Vector3 _origineRotation;
    private float _origineAlpha;
    private bool _isInit;

    public RectTransform Rt => _rt;
    public CanvasGroup Cg => _cg;

    public Vector2 OriginePosition => _originePosition;
    public Vector2 OriginalineScale => _origineScale;
    public Vector3 OrigineRotation => _origineRotation;
    public float OrigineAlpha => _origineAlpha;
    public bool IsInit => _isInit;
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (_cg == null)
            yield break;
        if (_rt == null)
            yield break;
        _origineAlpha = _cg.alpha;
        _originePosition = _rt.anchoredPosition;
        _origineScale = _rt.localScale;
        _origineRotation = _rt.localEulerAngles;
        _isInit = true;
        LateInit();
    }

    protected virtual void LateInit() { }
}

