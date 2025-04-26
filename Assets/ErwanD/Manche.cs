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
    private ScriptableKeyBind upBind;
    private ScriptableKeyBind downBind;
    private ScriptableKeyBind leftBind;
    private ScriptableKeyBind rightBind;
    
    public Manche(bool isHighTide, GameManager gameManager)
    {
        this.isHighTide = isHighTide;
        this.gameManager = gameManager;

    }

    void Start()
    {
        subscriseEvent();
        this.yellowAlgaes = this.gameManager.FindAllCaseType(Type.YellowAlgae);
        this.blueAlgaes = this.gameManager.FindAllCaseType(Type.BlueAlgae);
        StartCoroutine(StartTimer());
    }



    private void subscriseEvent()
    {
        this.upBind._onStart += upEvent;
    }

    public void upEvent()
    {

    }

    public void downEvent()
    {
    }

    public void leftEvent()
    {
    }
    public void rightEvent()
    {
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
