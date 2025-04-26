using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

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

    public Manche(GameManager gameManager, bool isHighTide)
    {
        _gameManager = gameManager;
        _isHighTide = isHighTide;
        this.yellowAlgaes = GameManager.Instance.FindAllCaseType(Type.YellowAlgae);
        this.blueAlgaes = GameManager.Instance.FindAllCaseType(Type.BlueAlgae);
        _gameManager.StartCoroutine(StartTimer());
        spawnEnemyZone();
    }

    private void spawnEnemyZone()
    {
       this.spawnedEnemy.Add(GameManager.Instance.enemyList[0]);
        foreach (Enemy enemy in this.spawnedEnemy)
        {

            Vector2Int randomSpawn = getRandomSpawn(enemy);
            for (int x = 0; x < enemy.width; x++)
            {
                for (int y = 0; y < enemy.height; y++)
                {
                    Debug.Log(" x et y :" + x + ", " + y);
                    Case c = new Case(enemy.tile, enemy.type, new Vector2Int(randomSpawn.x + x, randomSpawn.y + y));
                    GameManager.Instance.SetCaseBackground(c);
                }
            }
        }
    }

    public Vector2Int getRandomSpawn(Enemy enemy)
    {
        Case newC = new Case(Type.Black);
        Case newCLeft = new Case(Type.Black);
        Case newCTop = new Case(Type.Black);
        while ((newC != null && newC.type == Type.Black) ||
            (newCLeft != null && newCLeft.type == Type.Black) ||
            (newCTop != null && newCTop.type == Type.Black))
        {
            System.Random rnd = new System.Random();
            int randy = rnd.Next(1, 20 - enemy.height);
            int randx = rnd.Next(1, 20 - enemy.width);
            Vector2Int newVect = new Vector2Int(randx, randy);
            newC = GameManager.Instance.GetCase(newVect);
            newCLeft = GameManager.Instance.GetCase(new Vector2Int(newVect.x + enemy.width, newVect.y));
            newCTop = GameManager.Instance.GetCase(new Vector2Int(newVect.x, newVect.y +enemy.height));

        }
        return newC.position;
    }

    public void EndManche()
    {
        _gameManager.StopAllCoroutines();
    }

    IEnumerator StartTimer()
    {
        if (_isHighTide)
        {
            this.timer = highTideTime;
        }
        else
        {
            this.timer = lowTideTime;
        }
        while (this.timer > 0)
        {
            this.timer -= 1f;
            Debug.Log("Time Remaining: " + timer);
            yield return new WaitForSeconds(1f);
        }
        this.endTurn();
    }

    public IEnumerator moveDirectionPower(string direction, Type type)
    {
        List<Case> colorCase;
        if (type == Type.YellowAlgae)
        {
            colorCase = this.yellowAlgaes;
        }
        else if (type == Type.BlueAlgae)
        {
            colorCase = this.blueAlgaes;
        }
        else
        {
            yield break;
        }
        colorCase.Shuffle();
        foreach (Case c in colorCase)
        {
            Vector2Int numericDirection = ConvertDirection(direction);
            Debug.Log(c.position);
            Case nextCase = GameManager.Instance.GetCase(c.position + numericDirection);
            if (nextCase != null && nextCase.type == Type.Empty)
            {
                nextCase.type = c.type;
                nextCase.position = c.position + numericDirection;
                nextCase.tile = c.tile;
                yield return _gameManager.SetCase(nextCase);
            }
        }
    }

    public IEnumerator moveRandomDirection(string direction, Type type)
    {
        Vector2Int directionVector = ConvertDirection(direction);
        List<Case> possibleMove = FindAllEmpty(directionVector, type);
        
        int liberties = possibleMove.Count;
        int ran;
        Case chosenCase;
        Case newCase;
        for (int i = 0; i < liberties; i++)
        {
            ran = UnityEngine.Random.Range(0, possibleMove.Count);
            chosenCase = possibleMove[ran];
            newCase = new Case(
                chosenCase.tile,
                chosenCase.type,
                chosenCase.position + directionVector
            );
            yield return _gameManager.SetCase(newCase);
            if (_gameManager.GetCase(newCase.position + directionVector)?.type == Type.Empty)
            {
                possibleMove[ran] = newCase;
            }
            else
            {
                possibleMove.RemoveAt(ran);
            }
        }
    }

    public IEnumerator multiDirectionPower(Type type)
    {
        string[] directions = {"up", "down", "left", "right"};
        foreach (string direction in directions)
            yield return moveDirectionPower(direction, type);
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
        GameManager gm = GameManager.Instance;
        foreach (KeyValuePair<PowerType, int> entry in gm.cooldowns)
        {
            if (entry.Value > 0)
            {
                gm.setCooldown(entry.Key, entry.Value - 1);
            }
        }
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
