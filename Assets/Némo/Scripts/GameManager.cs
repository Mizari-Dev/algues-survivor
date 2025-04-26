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
    public Dictionary<PowerType, int> cooldowns;
    public List<Enemy> enemyList;

    public int turnCount = 0;
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
        cooldowns = new Dictionary<PowerType, int>()
        {
            {PowerType.Moove, 0},
            {PowerType.Random, 0},
            {PowerType.Bouclier, 0},
            {PowerType.UltiLigne, 0},
            {PowerType.UltiMultiple, 0},
        };
        InitBlackSquares();
        InitSpawn();
    }

    void Start()
    {
        Enemy[] objects = Resources.LoadAll<Enemy>("");
        enemyList = new List<Enemy>(objects);
        Debug.Log(enemyList.Count);
        currentManche = new Manche(this, false);
    }

    void Update()
    {
        
    }

    private void InitGrid()
    {
        Debug.Log($"x: {background.size.x}, y: {background.size.y}");
        _theoreticalMap = new Case[background.size.x][]; 
        for (int i = 0; i < _theoreticalMap.Length; i++)
        {
            _theoreticalMap[i] = new Case[background.size.y];
        }
    }

    private void InitBlackSquares()
    {
        for (int x = 0; x < background.size.x; x++)
        {
            for (int y = 0; y < background.size.y; y++)
            {
                TileBase tile = background.GetTile(new Vector3Int(x, y, 0));
                if (tile && tile is Tile)
                {
                    if ((tile as Tile).sprite.name == "Square")
                    {
                        SetCase(new Case(tile, Type.Black, new Vector2Int(x, y)));
                    }
                    else
                    {
                        SetCase(new Case(null, Type.Empty, new Vector2Int(x, y)));
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
    /// <param name="caseToSet">la case</param>
    public void SetCase(Case caseToSet)
    {
        
        _theoreticalMap[caseToSet.position.x][caseToSet.position.y] = caseToSet;
        if (caseToSet.tile)
            playground.SetTile(new Vector3Int(caseToSet.position.x, caseToSet.position.y), caseToSet.tile);
    }

    public void SetCaseBackground(Case caseToSet)
    {
        if (caseToSet.tile)
            background.SetTile(new Vector3Int(caseToSet.position.x, caseToSet.position.y), caseToSet.tile);
    }

    /// <summary>
    /// Récupérer la case
    /// </summary>
    /// <param name="position">position de la case</param>
    /// <returns>la case</returns>
    public Case GetCase(Vector2Int position)
    {
        try
        {
            return _theoreticalMap[position.x][position.y];
        }
        catch (Exception e)
        {
            return null;
        }
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
        this.upBind._onCancel += upCancelEvent;
        this.downBind._onCancel += downCancelEvent;
        this.leftBind._onCancel += leftCancelEvent;
        this.rightBind._onCancel += rightCancelEvent;
        this.yellow1Bind._onStart += yellow1Event;
        this.yellow2Bind._onStart += yellow2Event;
        this.yellow4Bind._onStart += firstUltiEvent;
        this.blue4Bind._onStart += secondUltiEvent;
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
        string direction = directionActive();
        if (direction != "")
        {
            this.currentManche.moveDirectionPower(direction, Type.YellowAlgae);
        }
        this.currentManche.endTurn();
    }

    private void yellow2Event()
    {
        string direction = directionActive();
        if (direction != "")
        {
            this.currentManche.moveRandomDirection(direction, Type.YellowAlgae);
        }
        this.currentManche.endTurn();
    }

    private void firstUltiEvent()
    {
        this.currentManche.multiDirectionPower(Type.YellowAlgae);
        this.currentManche.endTurn();
    }

    private void secondUltiEvent()
    {
        string direction = directionActive();
        if (direction != "")
        {
            this.currentManche.threeDirectionPower(direction, Type.YellowAlgae);
        }
        this.currentManche.endTurn();
    }

    string directionActive()
    {
        if (activeInput["right"])
        {
            return "right";
        } else if (activeInput["left"])
        {
            return "left";
        } else if (activeInput["up"])
        {
            return "up";
        } else if (activeInput["down"])
        {
            return "down";
        }
        return "";
    }

    public void nextTurn()
    {
        Debug.Log("NOUVEAU TOUR " + this.turnCount);
        turnCount += 1;
        currentManche.EndManche();
        currentManche = new Manche(this, false);
    }
    public void setCooldown(PowerType type, int time)
    {
        this.cooldowns[type] = time;
    }

    public int getCooldown(PowerType type)
    {
        return this.cooldowns[type];
    }
}
