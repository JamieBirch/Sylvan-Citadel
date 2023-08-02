using System;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public static event Action NewDay;
    
    public GameObject sun;
    
    public static int day;
    public float countdown;
    public float dayLength;
    
    public Text daysText;
    public Text countdownText;
    public Text timeText;
    
    // Start is called before the first frame update
    void Start()
    {
        // start world time
        countdown = dayLength;
        day = 1;
        
        NewDay?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        var sunTransform = sun.transform;

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        sunTransform.Rotate(Time.deltaTime * (360/dayLength),0, 0);
        
        if (countdown <= 0)
        {
            //start new day
            day++;
            countdown = dayLength;
            sunTransform.rotation = Quaternion.Euler(0, -60, 0);
            NewDay?.Invoke();
        }

        countdownText.text = $"{countdown:00.00}";
        daysText.text = day.ToString();

        int hours = (int)((1 - countdown/dayLength) * 24);
        TimeSpan time = new TimeSpan(hours, 0, 0);
        
        // DateTime dt = new DateTime((long)(dayLength - countdown));
        timeText.text = $"{time:t}";
    }
}
