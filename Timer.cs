using System;
using System.Collections;
using System.Collections.Generic;

public class Timer
{
    float time;
    float timer;
    bool autoRestart;
    public Action OnAction;

    public Timer(float _duration = 0, bool _autoRestart = true)
    {
        time = _duration;
        autoRestart = _autoRestart;

        Restart();
    }
    public void Tick(float tickTime)
    {
        timer -= tickTime;
        CheckForAction();
    }

    public void Restart() { timer = time; }
    public void SetParameters(float _duration, bool _autoRestart) 
    { 
        time = _duration;
        autoRestart = _autoRestart;
        Restart(); 
    }
    private void CheckForAction()
    {
        if (timer <= 0)
        {
            OnAction();
            if (autoRestart) Restart();
        }
    }
}

public enum TimerType
{
    ColorChangeGap,
    StackGap,
    BonusGap,
    BonusDuration,
    BetweenDiffGap,
    Amount
}
