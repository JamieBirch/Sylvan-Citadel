using System;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public static event Action NewDay;
    
    public GameObject sunRotationObject;
    // public Light sun;
    public Light moon;
    public float sunIntensityMultiplier;
    
    // TimeSpan dayStartTime = new TimeSpan(6, 0, 0);
    private int dayStartTime = 6;
    private int eveningTime = 20;
    
    public static int day;
    public float countdown;
    public float dayLength;
    
    public Text daysText;
    public Text countdownText;
    public Text timeText;

    private bool newDay;
    private Quaternion sunDefaultRotation;

    public Material windowMaterial;

    // private float moonIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        // sunDefaultRotation = sun.transform.rotation;
        
        // start world time
        countdown = dayLength;
        day = 1;
        
        newDay = true;
        // moonIntensity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        sunRotationObject.transform.Rotate(0,0, Time.deltaTime * (360/dayLength));
        
        float hours = (1 - countdown/dayLength) * 24;
        TimeSpan time = new TimeSpan((int)hours, 0, 0);
        
        // float sunIntensity = -(Mathf.Cos((float)hours / 4)) + 1;
        // float sunIntensity = (-(Mathf.Cos(hours / 4)) + 1)/2;
        // float moonIntensity = (Mathf.Cos(hours/4));
        // sunIntensity = Mathf.SmoothDamp(sunIntensity, newSunIntensity, , Time.deltaTime);
        
        // moon.intensity = sunIntensity * sunIntensityMultiplier;
        moon.intensity = Mathf.Cos(hours/4);

        if (countdown <= 0)
        {
            //start new day
            day++;
            countdown = dayLength;
            newDay = true;
        }

        if ((int)hours == dayStartTime && newDay)
        {
            NewDay?.Invoke();
            newDay = false;
            windowMaterial.DisableKeyword("_EMISSION");
        }

        if ((int)hours == eveningTime && !newDay)
        {
            windowMaterial.EnableKeyword("_EMISSION");
        }

        countdownText.text = $"{countdown:00.00}";
        daysText.text = day.ToString();

        timeText.text = $"{time:t}";
    }
}