using System;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public static Calendar instance;
    
    public static event Action NewDay;
    
    public GameObject sunRotationObject;
    public GameObject timeDiscObject;
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
    public static bool night = true;
    private Quaternion sunDefaultRotation;

    public Material windowMaterial;

    public float currentTimeScale = 1;
    public bool paused = false;

    // private float moonIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
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
        timeDiscObject.transform.Rotate(0,0, Time.deltaTime * (360/dayLength));
        
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
            night = false;
        }
        if ((int)hours == eveningTime && !newDay)
        {
            night = true;
        }

        //TODO move to House????
        if (night)
        {
            windowMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            windowMaterial.DisableKeyword("_EMISSION");
        }

        countdownText.text = $"{countdown:00.00}";
        daysText.text = day.ToString();

        timeText.text = $"{time:t}";
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        currentTimeScale = 0;
    }
    
    public void TimeScale1()
    {
        paused = false;
        Time.timeScale = 1f;
        currentTimeScale = 1;
    }
    
    public void TimeScale2()
    {
        paused = false;
        Time.timeScale = 2f;
        currentTimeScale = 2;
    }
    
    public void TimeScale4()
    {
        paused = false;
        Time.timeScale = 4f;
        currentTimeScale = 4;
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = currentTimeScale;
    }

    public float DayLength()
    {
        return dayLength;
    }
}
