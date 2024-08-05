using System;
using UnityEngine;

public class TimerUtils : MonoBehaviour
{
    public abstract class Timer
    {
        protected float initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => Time / initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public virtual void Start()
        {            
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);
    }

    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        public override void Start()
        {
            Reset();
            base.Start();
        }

        public bool IsFinished => Time <= 0;

        public void Reset() => Time = initialTime;

        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }

        public float GetTime() => Time;
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time < initialTime)
            {
                Time += deltaTime;
            }

            if (IsRunning && Time >= initialTime)
            {
                Stop();
            }
        }

        public override void Start()
        {
            Reset();
            base.Start();
        }

        public bool IsFinished => Time >= initialTime;

        public void Reset() => Time = 0;

        public float GetTime() => Time;
    }
}
