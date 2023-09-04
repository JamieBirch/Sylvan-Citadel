using System;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public static event Action NewDay;
    
    // public GameObject sun;
    public Light sun;
    
    // TimeSpan dayStartTime = new TimeSpan(6, 0, 0);
    private int dayStartTime = 6;
    
    public static int day;
    public float countdown;
    public float dayLength;
    
    public Text daysText;
    public Text countdownText;
    public Text timeText;

    private bool newDay;
    private Quaternion sunDefaultRotation;

    // private float sunIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        // sunDefaultRotation = sun.transform.rotation;
        
        // start world time
        countdown = dayLength;
        day = 1;
        
        newDay = true;
        // sunIntensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // var sunTransform = sun.transform;

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        // sunTransform.Rotate(Time.deltaTime * (270/dayLength),0, 0);
        
        float hours = (1 - countdown/dayLength) * 24;
        TimeSpan time = new TimeSpan((int)hours, 0, 0);
        // float sunIntensity = -(Mathf.Cos((float)hours / 4)) + 1;
        float newSunIntensity = (-(Mathf.Cos(hours / 4)) + 1)/2;
        // sunIntensity = Mathf.SmoothDamp(sunIntensity, newSunIntensity, , Time.deltaTime);
        
        sun.intensity = newSunIntensity;

        if (countdown <= 0)
        {
            //start new day
            day++;
            countdown = dayLength;
            // sunTransform.rotation = sunDefaultRotation;
            newDay = true;
        }

        if ((int)hours == dayStartTime && newDay)
        {
            NewDay?.Invoke();
            newDay = false;
        }

        countdownText.text = $"{countdown:00.00}";
        daysText.text = day.ToString();

        timeText.text = $"{time:t}";
    }
}
