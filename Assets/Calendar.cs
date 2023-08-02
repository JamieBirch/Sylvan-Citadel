using System;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public static event Action NewDay;
    
    public GameObject sun;
    
    public static int day;
    public float countdown;
    public float dayLength = 60f;
    
    public Text daysText;
    public Text countdownText;
    
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

        countdownText.text = string.Format("{0:00.00}", countdown);
        daysText.text = day.ToString();
    }
}
