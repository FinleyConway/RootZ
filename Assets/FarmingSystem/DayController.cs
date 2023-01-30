using System;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour
{
    private int _currentDay;

    private void Awake()
    {
        StartDay();
    }

    private void StartDay()
    {
        _currentDay++;
    }

    private void EndDay()
    {
    }

    public int GetCurrentDay() => _currentDay;
}
