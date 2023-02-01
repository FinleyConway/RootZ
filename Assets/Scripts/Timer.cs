using System;
using UnityEngine;

public class Timer
{
    private bool _isTimerRunning = false;
    private float _timer = 0;

    public event Action OnTimerFinish;

    public void TimerHandler(float delta)
    {
        if (!_isTimerRunning) return;

        _timer -= delta;
        if (_timer <= 0)
        {
            OnTimerFinish?.Invoke();
            _isTimerRunning = false;
        }
    }

    // turn on or off the timer
    public void InitTimer(bool state, float duration)
    {
        _timer = duration;
        _isTimerRunning = state;
    }

    public void ToggleTimer(bool state) => _isTimerRunning = state;
}
