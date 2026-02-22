using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CurrentTimeManager : MonoBehaviour
{
    public TextMeshProUGUI textClock , textDayOfWeek;
    
    void Update()
    {
        DateTime time = DateTime.Now;
        string hour = LeadingZero(time.Hour);
        string minute = LeadingZero(time.Minute);
        string second = LeadingZero(time.Second);
        textClock.text = hour + ":" + minute + ":" + second;
        DayOfWeek wk = DateTime.Today.DayOfWeek;
        textDayOfWeek.text = wk.ToString();
    }

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}