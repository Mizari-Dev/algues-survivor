using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private ScriptableKeyBind _upBind;
    [SerializeField] private ScriptableKeyBind _downBind;
    [SerializeField] private ScriptableKeyBind _leftBind;
    [SerializeField] private ScriptableKeyBind _rightBind;
    [SerializeField] private Vector2 _offset;
    private Vector2 _dir;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
        if (_upBind == null)
            return;       
        if (_downBind == null)
            return;        
        if (_leftBind == null)
            return;       
        if (_rightBind == null)
            return;
        _upBind._onStart += GoUp;
        _downBind._onStart += GoDown;
        _leftBind._onStart += GoLeft;
        _rightBind._onStart += GoRight;
        _upBind._onCancel += GoDown;
        _downBind._onCancel += GoUp;
        _leftBind._onCancel += GoRight;
        _rightBind._onCancel += GoLeft;
    }

    private void OnDestroy()
    {
        if (_upBind == null)
            return;
        if (_downBind == null)
            return;
        if (_leftBind == null)
            return;
        if (_rightBind == null)
            return;
        _upBind._onStart -= GoUp;
        _downBind._onStart -= GoDown;
        _leftBind._onStart -= GoLeft;
        _rightBind._onStart -= GoRight;
        _upBind._onCancel -= GoDown;
        _downBind._onCancel -= GoUp;
        _leftBind._onCancel -= GoRight;
        _rightBind._onCancel -= GoLeft;
    }

    private void GoUp()
    {
        _dir += Vector2.up;
        Refresh();
    }
    private void GoDown()
    {
        _dir += Vector2.down;
        Refresh();
    }
    private void GoRight()
    {
        _dir += Vector2.right;
        Refresh();
    }
    private void GoLeft()
    {
        _dir += Vector2.left;
        Refresh();
    }
    private void Refresh()
    {
        Vector3 currentCamPosition = _cam.transform.position;
        currentCamPosition = -_offset * _dir;
        currentCamPosition.z = _cam.transform.position.z;
        _cam.transform.position = currentCamPosition;
    }
}
