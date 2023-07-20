using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayClock: MonoBehaviour
{
    [SerializeField] private Transform hourArrow;
    [SerializeField] private Transform minuteArrow;
    [SerializeField] private Transform secondArrow;
    [SerializeField] private TMP_Text textTime;

    public void SetClock(DateTime time)
    {
        float currentSeconds = time.Second + time.Millisecond / 1000f;
        float currentMinutes = time.Minute + currentSeconds / 60;
        float currentHours = time.Hour + currentMinutes / 60;

        hourArrow.rotation = Quaternion.Euler(0, 0, CalculateRotate(currentHours, 12f));
        minuteArrow.rotation = Quaternion.Euler(0, 0, CalculateRotate(currentMinutes, 60f));
        secondArrow.rotation = Quaternion.Euler(0, 0, CalculateRotate(currentSeconds, 60f));

        textTime.text = $"{Math.Truncate(currentHours).ToString("00")}:{Math.Truncate(currentMinutes).ToString("00")}:{Math.Truncate(currentSeconds).ToString("00")}";
    }

    private float CalculateRotate(float count, float maxLimit)
    {
        return count / maxLimit * -360f;
    }
}
