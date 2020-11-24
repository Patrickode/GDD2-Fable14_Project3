using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    [SerializeField] RectTransform timerProgressBar = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [Space(10)]
    [Tooltip("How many diegetic (in-universe) minutes pass in one real-world second.")]
    [SerializeField] private float minuteSpeed = 1;
    [Tooltip("How long a day is, in diegetic (in-universe) minutes.")]
    [SerializeField] private float diegeticDayLength = 480;
    [Tooltip("The hour to start from, in a 24 hour format. Does not affect day length.")]
    [SerializeField] private int startHour = 9;
    [Tooltip("The minute to start from. Does not affect day length.")]
    [SerializeField] private int startMinute = 0;

    public float DayLength
    {
        get { return diegeticDayLength / minuteSpeed; }
        private set { diegeticDayLength = value; }
    }
    public float DayProgress { get; private set; } = 0;
    public float PercentThroughDay => DayProgress / DayLength;

    public static Action DayEnded;
    private bool dayEndEventInvoked = false;

    private void Start() { if (DayLength <= 0) { DayLength = float.MaxValue; } }

    private void Update()
    {
        //Add scaled time to day progress.
        DayProgress += Time.deltaTime;

        //Set up some counter variables to chunk DayProgress into minutes and seconds,
        //treated as hours and minutes respectively.
        int pseudoHours = startHour;
        int pseudoMinutes = startMinute + (int)DayProgress;
        bool isAM = true;

        //Subtract 60 from pseudoMinutes until it is between 0 and 59, and add one pseudoHour every time.
        while (pseudoMinutes >= 60)
        {
            pseudoMinutes -= 60;
            pseudoHours++;
        }
        //Toggle isAM whenever pseudohours hits a 12 hour interval.
        if (pseudoHours % 12 == 0)
        {
            isAM = false;
        }
        //Subtract 12 from pseudoHours until it's between 1 and 12, and toggle isAM every time.
        while (pseudoHours > 12)
        {
            pseudoHours -= 12;
            isAM = !isAM;
        }

        //Make the timer text show the hours, a colon, the minutes, a space, and then AM or PM based on isAM.
        timerText.text = $"{pseudoHours}:{pseudoMinutes:00} {(isAM ? "AM" : "PM")}";

        if (PercentThroughDay < 1)
        {
            //Make the timer progress bar equal the percentage of the way through the day.
            Vector3 newScale = timerProgressBar.localScale;
            newScale.x = PercentThroughDay;
            timerProgressBar.localScale = newScale;
        }
        else if (!dayEndEventInvoked)
        {
            DayEnded?.Invoke();
            dayEndEventInvoked = true;
        }
    }
}
