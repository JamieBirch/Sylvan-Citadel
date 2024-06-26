﻿using UnityEngine;

public class Windmill : Building
{
    public GameObject propeller;
    public int producePerDay;
    private float produceEvery_seconds;
    
    private float countdown;

    private void Start()
    {
        float dayLength = Calendar.instance.DayLength();
        produceEvery_seconds = dayLength / producePerDay;
        countdown = produceEvery_seconds;
    }

    private void Update()
    {
        //rotate propeller
        propeller.transform.Rotate(0,0, Time.deltaTime * 15);
        
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = produceEvery_seconds;
            GameStats.instance.AddFood();
            Debug.Log("Windmill produced some wheat");
        }
    }
}
