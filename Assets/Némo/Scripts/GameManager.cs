using System;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    private ScriptableKeyBind upBind;
    [SerializeField]
    private ScriptableKeyBind downBind;
    [SerializeField]
    private ScriptableKeyBind leftBind;
    [SerializeField]
    private ScriptableKeyBind rightBind;
    [SerializeField]
    private ScriptableKeyBind yellow1Bind;
    [SerializeField]
    private ScriptableKeyBind yellow2Bind;
    [SerializeField]
    private ScriptableKeyBind yellow3Bind;
    [SerializeField]
    private ScriptableKeyBind yellow4Bind;
    [SerializeField]
    private ScriptableKeyBind blue1Bind;
    [SerializeField]
    private ScriptableKeyBind blue2Bind;
    [SerializeField]
    private ScriptableKeyBind blue3Bind;
    [SerializeField]
    private ScriptableKeyBind blue4Bind;

    private Dictionary<string, bool> activeInput;
    private Manche currentManche;
    private Case[][] _theoreticalMap;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        InitGrid();
        subscriseEvent();
        activeInput = new Dictionary<string, bool>()
        {
            {"up", false},
            {"down", false},
            {"left", false},
            {"right", false}
        };
    }

    void Start()
    {
        InitBlackSquares();
        InitSpawn();
        currentManche = new Manche(false);
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
                TileBase tile = background.GetTile(new Vector3Int(x, y, 0));
                if (tile)
                {
                    if (tile is Tile)
                    {
                        if ((tile as Tile).sprite.name == "Square")
                        {
                            SetCase(new Case(tile, Type.Black, new Vector2Int(_x, _y)));
                        }
                        else
                        {
                            SetCase(new Case(null, Type.Empty, new Vector2Int(_x, _y)));
                        }
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
        if (caseToSet.tile)
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

    public List<Case> FindAllCaseType(Type type)
    {
        List<Case> resultTile = new List<Case>();
        for (int i = 0; i < _theoreticalMap.Length; i++)
        {
            Case[] lineCase = Array.FindAll(_theoreticalMap[i], c => c.type == type);
            resultTile.AddRange(lineCase);
        }
        
        return resultTile;
    }
    private void subscriseEvent()
    {
        this.upBind._onStart += upEvent;
        this.downBind._onStart += downEvent;
        this.leftBind._onStart += leftEvent;
        this.rightBind._onStart += rightEvent;
        this.upBind._onCancel += upEvent;
        this.downBind._onCancel += downEvent;
        this.leftBind._onCancel += leftEvent;
        this.rightBind._onCancel += rightEvent;
        this.yellow1Bind._onStart += yellow1Event;

    }

    private void AddDirectionEvent(string direction)
    {
        activeInput[direction] = true;
    }

    private void CancelDirectionEvent(string direction)
    {
        activeInput[direction] = false;
    }

    private void upEvent()
    {
        AddDirectionEvent("up");
    }

    private void downEvent()
    {
        AddDirectionEvent("down");
    }

    private void leftEvent()
    {
        AddDirectionEvent("left");
    }
    private void rightEvent()
    {
        AddDirectionEvent("right");
    }

    private void upCancelEvent()
    {
        CancelDirectionEvent("up");
    }

    private void downCancelEvent()
    {
        CancelDirectionEvent("down");
    }

    private void leftCancelEvent()
    {
        CancelDirectionEvent("left");
    }
    private void rightCancelEvent()
    {
        CancelDirectionEvent("right");
    }

    private void yellow1Event()
    {
        if (isDirectionActive())
        {
            this.currentManche.moveDirectionPower();
        }
    }

    bool isDirectionActive()
    {
        if (activeInput["right"] || activeInput["left"] || activeInput["up"] || activeInput["left"])
        {
            return true;
        }
        return false;
    }

}
