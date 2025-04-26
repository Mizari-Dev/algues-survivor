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

    public void moveDirectionPower()
    {

    }
}
