using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileBase _blackGrid;
    [SerializeField]
    public Tilemap background;
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
    public Type shieldedType;
    private bool _hasCastAction;
    private Coroutine timerCoroutine;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        InitGrid();
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
        Enemy[] objects = Resources.LoadAll<Enemy>("");
        enemyList = new List<Enemy>(objects);
        InitBlackSquares();
        InitSpawn();
        currentManche = new Manche(this, false);
        subscriseEvent();
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
                        StartCoroutine( SetCase(new Case(tile, Type.Black, new Vector2Int(x, y)),true));
                    }
                    else
                    {
                        StartCoroutine( SetCase(new Case(null, Type.Empty, new Vector2Int(x, y)),true));
                    }
                }
            }
        }
    }

    private void InitSpawn()
    {
        int x = 6;
        StartCoroutine( SetCase(new Case(yellowAlgae, Type.YellowAlgae, new Vector2Int(x, 1)),true));
        StartCoroutine( SetCase(new Case(blueAlgae, Type.BlueAlgae, new Vector2Int(x, _theoreticalMap[0].Length - 2)), true));
    }
    
    /// <summary>
    /// Créer la case
    /// </summary>
    /// <param name="caseToSet">la case</param>
    public IEnumerator SetCase(Case caseToSet, bool instant = false)
    {
        if (caseToSet == null)
            yield break;
        _theoreticalMap[caseToSet.position.x][caseToSet.position.y] = caseToSet;
        playground.SetTile(new Vector3Int(caseToSet.position.x, caseToSet.position.y), caseToSet.tile);
        if (!instant)
            yield return new WaitForSeconds(.02f);
    }

    public void SetCaseBackground(Case caseToSet)
    {
        if (caseToSet.tile)
            caseToSet.tile = _blackGrid;
        background.SetTile(new Vector3Int(caseToSet.position.x, caseToSet.position.y), caseToSet.tile);
    }

    /// <summary>
    /// Récupérer la case
    /// </summary>
    /// <param name="position">position de la case</param>
    /// <returns>la case</returns>
    public Case GetCase(Vector2Int position)
    {
        position.Clamp(Vector2Int.zero, new Vector2Int(_theoreticalMap.Length - 1, _theoreticalMap[0].Length - 1));
        try
        {
            return _theoreticalMap[position.x][position.y];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public IEnumerator DestroyCase(Vector2Int postiion)
    {
        Case tile = GetCase(postiion);
        if (tile == null)
            yield break;
        if (tile.type == shieldedType)
            yield break;
        if (tile.type == Type.BlueAlgae || tile.type == Type.YellowAlgae)
        {
            tile.type = Type.Empty;
            tile.tile = null;
            yield return SetCase(tile, true);
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
        this.yellow3Bind._onStart += yellow3Event;        
        this.blue1Bind._onStart += blue1Event;
        this.blue2Bind._onStart += blue2Event;
        this.blue3Bind._onStart += blue3Event;
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
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        StartCoroutine(yellow1EventInternal());
    }
    private IEnumerator yellow1EventInternal()
    {
        string direction = directionActive();
        if (direction != "")
        {
            yield return currentManche.moveDirectionPower(direction, Type.YellowAlgae);
            this.currentManche.endTurn();
        }
    }
    private void yellow2Event()
    {
        if (_hasCastAction || this.getCooldown(PowerType.Random) > 0)
            return;
        _hasCastAction = true;
        StartCoroutine(yellow2EventInternal());
    }
    private IEnumerator yellow2EventInternal()
    {
        string direction = directionActive();
        if (direction != "")
        {
            yield return currentManche.moveRandomDirection(direction, Type.YellowAlgae);
            this.setCooldown(PowerType.Random, 1);
            this.currentManche.endTurn();
        }
    }

    private void yellow3Event()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        shieldedType = Type.YellowAlgae;
        this.currentManche.endTurn();
    }
    private void blue1Event()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        StartCoroutine(blue1EventInternal());
    }
    private IEnumerator blue1EventInternal()
    {
        string direction = directionActive();
        if (direction != "")
        {
            yield return currentManche.moveDirectionPower(direction, Type.BlueAlgae);
            this.currentManche.endTurn();
        }
    }
    private void blue2Event()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        StartCoroutine(blue2EventInternal());
    }
    private IEnumerator blue2EventInternal()
    {
        string direction = directionActive();
        if (direction != "")
        {
            yield return currentManche.moveRandomDirection(direction, Type.BlueAlgae);
            this.currentManche.endTurn();
        }
    }
    private void blue3Event()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        shieldedType = Type.BlueAlgae;
        this.currentManche.endTurn();
    }
    private void firstUltiEvent()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        StartCoroutine(firstUltiEventInternal());
    }
    private IEnumerator firstUltiEventInternal()
    {
        yield return currentManche.multiDirectionPower(Type.YellowAlgae);
        yield return currentManche.multiDirectionPower(Type.BlueAlgae);
        this.currentManche.endTurn();
    }
    private void secondUltiEvent()
    {
        if (_hasCastAction)
            return;
        _hasCastAction = true;
        StartCoroutine(secondUltiEventInternal());
    }
    private IEnumerator secondUltiEventInternal()
    {
        string direction = directionActive();
        if (direction != "")
        {
            yield return currentManche.threeDirectionPower(direction, Type.YellowAlgae);
            yield return currentManche.threeDirectionPower(direction, Type.BlueAlgae);
            this.currentManche.endTurn();
        }
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
        StartCoroutine(NextTurnInternal());
    }

    private IEnumerator NextTurnInternal()
    {
        yield return currentManche.EndManche();
        currentManche = new Manche(this, false);
        _hasCastAction = false;
        shieldedType = Type.Empty;
    }
    public void setCooldown(PowerType type, int time)
    {
        this.cooldowns[type] = time;
    }

    public int getCooldown(PowerType type)
    {
        return this.cooldowns[type];
    }
    
    public void stopTimer()
    {
        StopCoroutine(this.timerCoroutine);
    }
 
    public void startTimer()
    {
        this.timerCoroutine = StartCoroutine(this.currentManche.StartTimer());
    }
}
