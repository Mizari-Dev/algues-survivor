using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Manche : MonoBehaviour
{
    public bool isHighTide = false;
    private int highTideTime = 15;
    private int lowTideTime = 8;
    private float timer;
    private List<Case> yellowAlgaes;
    private List<Case> blueAlgaes;
    private GameManager gameManager;
    
    public Manche(bool isHighTide)
    {
        this.isHighTide = isHighTide;
    }
    void Awake()
    {

    }

    void Start()
    {
        this.yellowAlgaes = GameManager.Instance.FindAllCaseType(Type.YellowAlgae);
        this.blueAlgaes = GameManager.Instance.FindAllCaseType(Type.BlueAlgae);
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (this.timer > 0)
        {
            this.timer -= 1f;
            Debug.Log("Time Remaining: " + timer);
            yield return new WaitForSeconds(1f);
        }
    }

    public void moveDirectionPower(string direction, string color)
    {
        List<Case> colorCase;
        if (color == "yellow")
        {
            colorCase = this.yellowAlgaes;
        } else {
            colorCase = this.blueAlgaes;
        }

        foreach (Case c in colorCase)
        {
           Vector2Int numericDirection = ConvertDirection(direction);
            try
            {
                Case nextCase = GameManager.Instance.GetCase(c.position + numericDirection);
                if (nextCase != null)
                {
                    nextCase.type = c.type;
                    nextCase.position = c.position + numericDirection;
                    nextCase.tile = c.tile;
                    GameManager.Instance.SetCase(nextCase);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void moveRandomDirection(string direction)
    {
        Vector2Int directionVector = ConvertDirection(direction);
        List<Case> possibleMove = new List<Case>();
        Case[][] theoreticalMap = GameManager.Instance.TheoreticalMap;
        possibleMove = FindAllEmpty(directionVector);
        
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
            GameManager.Instance.SetCase(newCase);
            if (GameManager.Instance.GetCase(chosenCase.position + directionVector).type == Type.Empty)
            {
                possibleMove[ran] = newCase;
            }
            else
            {
                possibleMove.RemoveAt(ran);
            }
        }
    }

    public void multiDirectionPower()
    {
        
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

    private List<Case> FindAllEmpty(Vector2Int direction)
    {
        Case[][] theoreticalMap = GameManager.Instance.TheoreticalMap;
        List<Case> empties = new List<Case>();
        for (int i = 0; i < theoreticalMap.Length; i++)
        {
            empties.AddRange(Array.FindAll(
                theoreticalMap[i], 
                caseToFind => GameManager.Instance.GetCase(caseToFind.position).type == Type.Empty));
        }

        return empties;
    }
}
