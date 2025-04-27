using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class Manche
{
    public bool _isHighTide = false;
    private int highTideTime = 15;
    private int lowTideTime = 8;
    private float timer;
    private List<Case> yellowAlgaes;
    private List<Case> blueAlgaes;
    private GameManager _gameManager;
    private List<Enemy> spawnedEnemy = new List<Enemy>();
    private List<Case> ennemyCases = new List<Case>();
    private List<Type> fishList = new List<Type> {Type.Fish,Type.Fish2,Type.Fish3,Type.ArgZilla,Type.Coquillage,Type.Fugu,Type.Couteau,Type.Palourde}; 
    
    public Manche(GameManager gameManager, bool isHighTide)
    {
        _gameManager = gameManager;
        _isHighTide = isHighTide;
        this.yellowAlgaes = _gameManager.FindAllCaseType(Type.YellowAlgae);
        this.blueAlgaes = _gameManager.FindAllCaseType(Type.BlueAlgae);
        spawnEnemyZone();
    }

    private void selectEnnemies()
    {
        if(_gameManager.turnCount % 5 == 0)
        {
            _gameManager.ennemiesNumber += 3;
        }

        this.spawnedEnemy = new List<Enemy>();
        for (int i = 0; i < _gameManager.ennemiesNumber; i++)
        {
            List<Enemy> enemies = new List<Enemy>();
            if (_isHighTide)
            {
                System.Random rndGodz = new System.Random();
                int randGodz = rndGodz.Next(0,36);
                if(randGodz == 1)
                {
                    enemies = _gameManager.enemyList.Where(enemy => enemy.type == Type.ArgZilla).ToList();
                }
                else
                {
                    enemies = _gameManager.enemyList.Where(enemy => enemy.isHighTide && !(enemy.type == Type.ArgZilla)).ToList();
                }
            }
            else
            {
                enemies = _gameManager.enemyList.Where(enemy => !enemy.isHighTide).ToList();
            }
            if (enemies.Count == 0)
                return;
            System.Random rnd = new System.Random();
            int rand = rnd.Next(0, enemies.Count);
            this.spawnedEnemy.Add(enemies[rand]);
        }
    }

    private void spawnEnemyZone()
    {
        selectEnnemies();
        foreach (Enemy enemy in this.spawnedEnemy)
        {
            Debug.Log(enemy.name);
            Vector2Int randomSpawn = getRandomSpawn(enemy);
            for (int x = 0; x < enemy.width; x++)
            {
                for (int y = 0; y < enemy.height; y++)
                {
                    Case c = new Case(enemy.tile, enemy.type, new Vector2Int(randomSpawn.x + x, randomSpawn.y + y));
                    ennemyCases.Add(c);
                    GameManager.Instance.SetCaseBackground(c);
                }
            }
            _gameManager.EnnemyManager.AddEnemy(enemy, randomSpawn);
        }
    }

    public bool isFish(Type type)
    {
        return this.fishList.Contains(type);
    }

    public Vector2Int getRandomSpawn(Enemy enemy)
    {
        int iterateCount = 0;
        Case newC = new Case(Type.Black);
        Case newCLeft = new Case(Type.Black);
        Case newCTop = new Case(Type.Black);
        while ((newC != null && newC.type == Type.Black) ||
            (newCLeft != null && newCLeft.type == Type.Black) ||
            (newCTop != null && newCTop.type == Type.Black) ||
            (newC != null && isFish(newC.type)) ||
            (newCLeft != null && isFish(newCLeft.type)) ||
            (newCTop != null && isFish(newCTop.type)) 
            )
        {
            iterateCount++;
            System.Random rnd = new System.Random();
            int randy = rnd.Next(0, _gameManager.background.size.y - enemy.height+1);
            int randx = rnd.Next(0, _gameManager.background.size.x - enemy.width+1);
            Vector2Int newVect = new Vector2Int(randx, randy);
            newC = _gameManager.GetCase(newVect);
            newCLeft = _gameManager.GetCase(new Vector2Int(newVect.x + enemy.width, newVect.y));
            newCTop = _gameManager.GetCase(new Vector2Int(newVect.x, newVect.y +enemy.height));
            if(iterateCount == 10)
            {
                return new Vector2Int(0,0);
            }
        }
        return newC.position;
    }

    public IEnumerator StartTimer()
    {
        if (_isHighTide)
        {
            this.timer = highTideTime;
        }
        else
        {
            this.timer = lowTideTime;
        }
        Events.SetTimer(timer);
        while (this.timer > 0)
        {
            this.timer -= 1f;
           // Debug.Log("Time Remaining: " + timer);
            yield return new WaitForSeconds(1f);
            Events.SetTimer(timer);
        }
        this.endTurn();
    }

    public IEnumerator moveDirectionPower(string direction, Type type)
    {
        List<Case> empties = FindAllEmpty(ConvertDirection(direction), type);
        Vector2Int numericDirection = ConvertDirection(direction);
        int numberOfMove = Math.Clamp(empties.Count, 0, 2);
        for (int i = 0; i < numberOfMove; i++)
        {
            Case c = empties[Random.Range(0, empties.Count)];
            Case nextCase = GameManager.Instance.GetCase(c.position + numericDirection);
            nextCase.type = c.type;
            nextCase.position = c.position + numericDirection;
            nextCase.tile = c.tile;
            empties.Remove(c);
            yield return _gameManager.SetCase(nextCase);
        }
    }

    public IEnumerator moveRandomDirection(Type type)
    {
        List<Vector2Int> directions = new List<Vector2Int>()
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };
        List<List<Case>> possibleMove = new List<List<Case>>()
        {
            FindAllEmpty(Vector2Int.down, type),
            FindAllEmpty(Vector2Int.up, type),
            FindAllEmpty(Vector2Int.left, type),
            FindAllEmpty(Vector2Int.right, type)
        };
        
        int liberties = 0;
        for (int i = 0; i < possibleMove.Count; i++)
        {
            if (possibleMove[i].Count == 0)
            {
                possibleMove.RemoveAt(i);
                directions.RemoveAt(i);
                i--;
                continue;
            }
            liberties += possibleMove[i].Count;
        }
        
        List<Case> color;
        if (type == Type.YellowAlgae)
            color = this.yellowAlgaes;
        else
            color = this.blueAlgaes;
        
        liberties = Math.Clamp(liberties, 0, Math.Clamp(color.Count,0,5));
        int ran;
        int ranDir;
        Case chosenCase;
        Case newCase;
        for (int i = 0; i < liberties; i++)
        {
            ranDir = Random.Range(0, possibleMove.Count);
            ran = Random.Range(0, possibleMove[ranDir].Count);
            chosenCase = possibleMove[ranDir][ran];
            newCase = new Case(
                chosenCase.tile,
                chosenCase.type,
                chosenCase.position + directions[ranDir]
            );
            possibleMove[ranDir].RemoveAt(ran);
            if (possibleMove[ranDir].Count == 0)
            {
                possibleMove.RemoveAt(ranDir);
                directions.RemoveAt(ranDir);
            }
            yield return _gameManager.SetCase(newCase);
        }
        _gameManager.setCooldown(PowerType.Random, 5);
    }

    public IEnumerator multiDirectionPower(Type type)
    {
        Vector2Int[] direcitons = new Vector2Int[]
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };
        
        List<List<Case>> empties = new List<List<Case>>()
        {
            FindAllEmpty(Vector2Int.down, type),
            FindAllEmpty(Vector2Int.up, type),
            FindAllEmpty(Vector2Int.left, type),
            FindAllEmpty(Vector2Int.right, type)
        };
        
        for (int i = 0; i < empties.Count; i++)
        {
            for (int j = 0; j < empties[i].Count; j++)
            {
                Case c = empties[i][j];
                Case nextCase = _gameManager.GetCase(c.position + direcitons[i]);
                nextCase.type = c.type;
                nextCase.position = c.position + direcitons[i];
                nextCase.tile = c.tile;
                yield return _gameManager.SetCase(nextCase);
            }
        }
    }

    public IEnumerator threeDirectionPower(string direction, Type type)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return moveDirectionPower(direction, type);
            if (type == Type.YellowAlgae)
                this.yellowAlgaes = GameManager.Instance.FindAllCaseType(Type.YellowAlgae);
            else
                this.blueAlgaes = GameManager.Instance.FindAllCaseType(Type.BlueAlgae);
        }
    }

    private Vector2Int ConvertDirection(string direction)
    {
        int x = 0;
        int y = 0;
        switch (direction)
        {
            case "up":
                y = 1;
                break;
            case "down":
                y = -1;
                break;
            case "left":
                x = -1;
                break;
            case "right":
                x = 1;
                break;
            default:
                break;
        }
        
        return new Vector2Int(x, y);
    }

    public void endTurn()
    {
        _gameManager.stopTimer();
        GameManager gm = GameManager.Instance;

        gm.cooldowns.Where(c => c.Value > 0).Select(c => (c.Key, c.Value - 1)).ToList().ForEach(c => gm.setCooldown(c.Key, c.Item2));

        gm.nextTurn();
    }

    private List<Case> FindAllEmpty(Vector2Int direction, Type type)
    {
        List<Case> algaes;
        if (type == Type.YellowAlgae)
            algaes = this.yellowAlgaes;
        else if (type == Type.BlueAlgae)
            algaes = this.blueAlgaes;
        else
            return null;
        
        return algaes.FindAll(
                caseToFind => GameManager.Instance.GetCase(caseToFind.position + direction)?.type == Type.Empty);
    }

    private bool isOnCooldown(PowerType type)
    {
        return GameManager.Instance.getCooldown(type) != 0;
    }
}
