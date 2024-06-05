
using UnityEngine;

public class Orchard : Building
{
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
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = produceEvery_seconds;
            GameStats.instance.AddFood();
            Debug.Log("Orchard produced some food");
        }
    }
}
