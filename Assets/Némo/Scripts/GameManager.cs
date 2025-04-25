using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap _background;
    [SerializeField]
    private Tilemap _playground;
    private List<List<Case>> _theroical_map;
    
    void Awake()
    {
        _theroical_map = new List<List<Case>>(_background.size.y);
        for (int i = 0; i < _theroical_map.Count; i++)
        {
            _theroical_map[i] = new List<Case>(_background.size.x);
        }
    }

    void Start()
    {
        InitBlackSquares();
    }

    void Update()
    {
        
    }

    private void InitBlackSquares()
    {
        for (int y = (int)(0 - _background.size.y * .5f); y < _background.size.y; y++)
        {
            for (int x = (int)(0 - _background.size.x * .5f); x < _background.size.x; x++)
            {
                Sprite _sprite = _background.GetSprite(new Vector3Int(x, y, 0));
                if (_sprite)
                    if (_sprite.name == "Square")
                    {
                        _theroical_map[x][y] = new Case(_sprite, Type.Black);
                    }
                    else
                    {
                        _theroical_map[x][y] = new Case(null, Type.Empty);
                    }
            }
        }
    }
}
