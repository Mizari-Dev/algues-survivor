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
    private AnimatedTile yellowAlgae;
    [SerializeField]
    private AnimatedTile blueAlgae;

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
                        _theoreticalMap[_x][_y] = new Case(tile, Type.Black, new Vector2Int(x, y));
                    }
                    else
                    {
                        _theoreticalMap[_x][_y] = new Case(null, Type.Empty, new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    private void InitSpawn()
    {
        int x = (int)(_theoreticalMap.Length * .5f);
        SetCase(new Case(yellowAlgae, Type.YellowAlgae, new Vector2Int(x, 1)));
        SetCase(new Case(blueAlgae, Type.BlueAlgae, new Vector2Int(x, _theoreticalMap[0].Length - 2)));
    }

    /// <summary>
    /// Créer la case
    /// </summary>
    /// <param name="position">position de la case</param>
    /// <param name="caseToSet">la case</param>
    public void SetCase(Case caseToSet)
    {
        Vector3Int offsetPosition = new Vector3Int(
            (int)(caseToSet.position.x - background.size.x * .5f),
            (int)(caseToSet.position.y - background.size.y * .5f),
            0
        );
        
        _theoreticalMap[caseToSet.position.x][caseToSet.position.y] = caseToSet;
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

    public List<Case> findAllCaseType(Type type)
    {
        List<Case> resultTile = new List<Case>();
        for (int i = 0; i < _theoreticalMap.Length; i++)
        {
            Case[] lineCase = Array.FindAll(_theoreticalMap[i], c => c.type == type);
            resultTile.AddRange(lineCase);
        }
        
        return resultTile;
    }

            void GetTilesWithSpecificSprites(Sprite sprite1, Sprite sprite2)
    {
        BoundsInt bounds = this.playground.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            TileBase tile = this.playground.GetTile(position);

            if (tile != null && tile is Tile)
            {
                Tile tileObj = tile as Tile;

                if (tileObj.sprite == sprite1 || tileObj.sprite == sprite2)
                {
                    Debug.Log("Tile  " + position);

                }
            }
        }
    }

    public List<Case> GetYellowAlgue()
    {

    }
}
