using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap background;
    [SerializeField]
    private Tilemap playground;
    private Case[][] _theoreticalMap;
    
    void Awake()
    {
        InitGrid();
    }

    void Start()
    {
        InitBlackSquares();
        InitSpawn();
    }

    void Update()
    {
        
    }

    private void InitGrid()
    {
        _theoreticalMap = new Case[background.size.x][];
        for (int i = 0; i < _theoreticalMap.Length; i++)
        {
            _theoreticalMap[i] = new Case[background.size.y];
        }
    }

    private void InitBlackSquares()
    {
        int xMid = (int)(background.size.x * .5f);
        int yMid = (int)(background.size.y * .5f);
        for (int y = 0 - yMid; y < yMid; y++)
        {
            for (int x = 0 - xMid; x < xMid; x++)
            {
                int _x = x + xMid;
                int _y = y + yMid;
                Sprite sprite = background.GetSprite(new Vector3Int(x, y, 0));
                if (sprite)
                {
                    if (sprite.name == "Square")
                    {
                        _theoreticalMap[_x][_y] = new Case(sprite, Type.Black);
                    }
                    else
                    {
                        _theoreticalMap[_x][_y] = new Case(null, Type.Empty);
                    }
                }
            }
        }
    }

    private void InitSpawn()
    {
        
    }
}
