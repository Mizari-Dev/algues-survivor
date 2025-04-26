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
    
    public Manche(bool isHighTide)
    {
        this.isHighTide = isHighTide;

    }

    private void Start()
    {
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
