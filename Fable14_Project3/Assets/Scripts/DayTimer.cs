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
    [Tooltip("How long a day is in seconds. One real world second = one in-universe minute.")]
    [SerializeField] private float dayLength = 540;
    [Tooltip("The hour to start from, in a 24 hour format. Does not affect day length.")]
    [SerializeField] private int startHour = 9;
    [Tooltip("The minute to start from. Does not affect day length.")]
    [SerializeField] private int startMinute = 0;

    public float DayLength { get { return dayLength; } private set { dayLength = value; } }
    public float DayProgress { get; private set; } = 0;
    public float PercentThroughDay => DayProgress / DayLength;

    public static Action DayEnded;
    private bool dayEndEventInvoked = false;

    private void Start() { if (DayLength <= 0) { DayLength = float.MaxValue; } }

    private void Update()
    {
        if (PercentThroughDay < 1)
        {
            //Add scaled time to day progress.
            DayProgress += Time.deltaTime;

            //Make the timer progress bar equal the percentage of the way through the day.
            Vector3 newScale = timerProgressBar.localScale;
            newScale.x = PercentThroughDay;
            timerProgressBar.localScale = newScale;

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
        }
        else if (!dayEndEventInvoked)
        {
            DayEnded?.Invoke();
            dayEndEventInvoked = true;
        }
    }
}
