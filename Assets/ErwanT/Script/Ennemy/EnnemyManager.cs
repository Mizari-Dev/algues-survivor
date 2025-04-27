using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    [SerializeField] private EnnemyRender _prefab;
    private List<EnnemyRender> _visual = new List<EnnemyRender>();

    public void AddEnemy(Enemy enemy, Vector2Int position)
    {
        EnnemyRender visual = Instantiate(_prefab, transform);
        visual.Populate(enemy, position);
        _visual.Add(visual);
    }

    public IEnumerator Play()
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        for (int i = 0; i < _visual.Count; i++)
            coroutines.Add(StartCoroutine(_visual[i].Play()));
        foreach (var corout in coroutines)
            yield return corout;
        foreach (Transform T in transform)
            Destroy(T.gameObject);
        _visual.Clear();
    }
}
