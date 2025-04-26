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
    
    public Manche(bool isHighTide, GameManager gameManager)
    {
        this.isHighTide = isHighTide;
        this.gameManager = gameManager;

    }

    private void Start()
    {
        this.yellowAlgaes = this.gameManager.findAllCaseType(Type.YellowAlgae);
        this.blueAlgaes = this.gameManager.findAllCaseType(Type.BlueAlgae);
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
}
