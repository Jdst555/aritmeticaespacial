using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public Image TimerUI;

    private float period = 0;
    private float elapsedTime = 0;
    private float remainingTime = 0;
    private float startTime = 0;
    private bool isOn = false;
    private Color green = new Color(0,1,0,1);
    private Color red = new Color(1,0,0.836f,1);
    private TextMeshProUGUI counter;
    private float colorHue;
    private float timerInterpolation = 0;

    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    private void Awake()
    {
        TimerUI = GetComponent<Image>();
        counter = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        ResetTimer();
    }
    void FixedUpdate()
    {
        if (isOn)
        {
            if (ElapsedPeriod())
            {
                OnTimeOver?.Invoke();
                ResetTimer();
            }

        }
    }
    public void StartTimer(float period)
    {
        ResetTimer();
        this.period = period;
        isOn = true;
    }
    public void PauseUnpauseTimer()
    {
        if (isOn)
        {
            isOn = false;
        }
        else { isOn = true; }
    }
    private bool ElapsedPeriod()
    {
        elapsedTime += Time.fixedDeltaTime;
        TimerUpdate();
        if (ElapsedTime >= period)
        {
            return true;
        }
        else { return false; }
    }
    private void ResetTimer()
    {
        TimerUI.fillAmount = 1;
        TimerUI.color = Color.HSVToRGB((120 / 360), 1, 1); 
        elapsedTime = 0;
        remainingTime = period;
        colorHue = 120/360;
        isOn = false;

    }
    private void TimerUpdate()
    {
        //actualizar contador del timer
        remainingTime = period - elapsedTime;
            
        float minutes = Mathf.FloorToInt( remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        //Debug.Log("TIMER_UPDATE. Minutes: " + minutes + " Seconds: " + seconds);
        if (remainingTime >= 0)
        {
            
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            counter.SetText(formattedTime);
            //Debug.Log("Formatted time string: " + formattedTime);
        }
       
        //actulizar imagen del timer
        //colorValue interpolar colorValue de 120 a 0. Mapear de 1 a 0;
        
        timerInterpolation = remainingTime / period;
        colorHue = (120f / 360f) * timerInterpolation;
        TimerUI.color = Color.HSVToRGB(colorHue, 1, 1);
        TimerUI.fillAmount = timerInterpolation;
  
    }
    public delegate void TimeOver();
    public static event TimeOver OnTimeOver;
}
