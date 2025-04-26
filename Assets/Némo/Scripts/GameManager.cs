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
    [SerializeField]
    private Tile algae1;
    [SerializeField]
    private Tile algae2;

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
                Tile tile = background.GetTile<Tile>(new Vector3Int(x, y, 0));
                if (tile)
                {
                    if (tile.sprite.name == "Square")
                    {
                        _theoreticalMap[_x][_y] = new Case(tile, Type.Black);
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
        int x = (int)(_theoreticalMap.Length * .5f);
        SetCase(new Vector3Int(x, 1, 0), new Case(algae1, Type.Algae1));
        SetCase(new Vector3Int(x, _theoreticalMap[0].Length-2, 0), new Case(algae2, Type.Algae2));
    }

    /// <summary>
    /// Créer la case
    /// </summary>
    /// <param name="position">position de la case</param>
    /// <param name="caseToSet">la case</param>
    public void SetCase(Vector3Int position, Case caseToSet)
    {
        Vector3Int offsetPosition = new Vector3Int(
            (int)(position.x - background.size.x * .5f),
            (int)(position.y - background.size.y * .5f),
            0
        );
        
        _theoreticalMap[position.x][position.y] = caseToSet;
        playground.SetTile(offsetPosition, caseToSet.tile);
    }

    /// <summary>
    /// Récupérer la case
    /// </summary>
    /// <param name="position">position de la case</param>
    /// <returns>la case</returns>
    public Case GetCase(Vector3Int position)
    {
        return _theoreticalMap[position.x][position.y];
    }
}
