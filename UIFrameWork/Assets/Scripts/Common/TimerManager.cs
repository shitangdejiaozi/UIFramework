using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerManager : Singleton<TimerManager> {

    private uint m_timerSequence = 1;
    private List<Timer> m_timers = new List<Timer>(10);
    public uint AddTimer(float interval, Action<Timer> handler, int loop = 1)
    {
        m_timerSequence++;
        Timer timer = new Timer(interval, handler, loop, m_timerSequence);
        m_timers.Add(timer);
        return m_timerSequence;
    }

    public uint AddTimer(float interval, Action handler, int loop = 1)
    {
        Debug.LogError("Addtimer");
        m_timerSequence++;
        Action<Timer> h = (t) => { handler(); };
        Timer timer = new Timer(interval, h, loop, m_timerSequence);
        m_timers.Add(timer);
        return m_timerSequence;
    }

    public void KillTimer(uint sequence)
    {
        for(int i = 0; i< m_timers.Count; i++)
        {
            if(m_timers[i].IsSequenceEqual(sequence))
            {
                m_timers[i].Finish();
                break;
            }
        }
    }

    public void KillAllTimer()
    {
        m_timers.Clear();
    }

    public void UpdateTimer(float delta)
    {
        for(int i = 0; i< m_timers.Count;)
        {
            if(m_timers[i].IsFinished())
            {
                m_timers.RemoveAt(i);
                continue;
            }

            m_timers[i].Update(delta);
            i++;
        }
    }

    public Timer GetTimer(uint sequence)
    {
        for (int i = 0; i < m_timers.Count; i++)
        {
            if (m_timers[i].IsSequenceEqual(sequence))
            {
                return m_timers[i];
            }
        }
        return null;
    }

    public void  PauseTimer(uint sequence)
    {
        Timer timer = GetTimer(sequence);
        timer.PauseTimer();
    }

    public void ResetTimer(uint sequence)
    {
        Timer timer = GetTimer(sequence);
        timer.ResetTimer();
    }
}

public class Timer
{
    private Action<Timer> m_handler;
    private float m_interval;
    private int m_loop;
    private uint m_sequence;

    private float m_currentTime;
    private float m_totalTime;

    private bool m_isFinished = false;
    private bool m_isRunning = false;

    public Timer(float interval, Action<Timer> handler, int loop, uint sequence)
    {
        m_interval = interval;
        m_handler = handler;
        m_loop = loop;
        m_sequence = sequence;

        m_totalTime = interval;
        m_currentTime = 0;
        m_isFinished = false;
        m_isRunning = true;
    }

    public void Update(float delta)
    {
        if(m_isFinished || !m_isRunning)
        {
            return;
        }
        m_currentTime += delta;
        
        
        if (m_currentTime >= m_totalTime)
        {
            m_loop--;
            m_currentTime -= m_totalTime;
            if (m_handler != null)
            {
                Debug.LogError("handleer");
                m_handler(this);
            }
            if (m_loop == 0)
                m_isFinished = true;
        }
        
       
    }

    public bool IsFinished()
    {
        return m_isFinished;
    }

    public void Finish()
    {
        m_isFinished = true;
    }

    public bool IsSequenceEqual(uint sequence)
    {
        return m_sequence == sequence;
    }

    public void PauseTimer()
    {
        m_isRunning = false;
    }

    public void ResetTimer()
    {
        m_isRunning = true;
    }

    public float GetLeftTime()
    {
        return m_totalTime - m_currentTime;
    }
}
