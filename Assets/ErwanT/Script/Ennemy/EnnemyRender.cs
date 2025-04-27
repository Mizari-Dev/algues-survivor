using UnityEngine;
using System.Collections;

public class EnnemyRender : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
    private GameManager _gameManager;
    private Enemy _enemy;
    private Vector2Int _position;
    private bool _inAnim;
    public void Populate(Enemy enemy, Vector2Int position)
    {
        _gameManager = GameManager.Instance;
        _enemy = enemy;
        _position = position;
        _renderer.sprite = enemy.image;
        transform.position = GetPosition(position);
        _animator.runtimeAnimatorController = enemy._runtimeAnimatorController;
        if (enemy._soundOnSpawn != null)
            SoundManager.Instance.PlaySound(enemy._soundOnSpawn);
    }

    private Vector2 GetPosition(Vector2 position)
    {
        Vector2 fPos = Vector2.zero;
        for (int x = 0; x < _enemy.width; x++)
            for (int y = 0; y < _enemy.height; y++)
                fPos += new Vector2Int(_position.x + x, _position.y + y);
        fPos /= _enemy.width * _enemy.height;
        fPos = _offset + fPos / 2;
        return fPos;
    }

    public IEnumerator Play()
    {
        _inAnim = true;
        _animator.SetTrigger("Play");
        yield return new WaitWhile(() => _inAnim);

    }

    public void OnEndAnim()
    {
        StartCoroutine(OnEndAnimInternal());
    }
    public IEnumerator OnEndAnimInternal()
    {
        for (int x = 0; x < _enemy.width; x++)
            for (int y = 0; y < _enemy.height; y++)
            {
                Vector2Int position = new Vector2Int(_position.x + x, _position.y + y);
                yield return _gameManager.DestroyCase(position);
                _gameManager.SetCaseBackground(new Case(null, Type.Empty, position));
            }
        _inAnim = false;
        Destroy(gameObject);
    }
}
