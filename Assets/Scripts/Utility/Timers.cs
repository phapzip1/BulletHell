using System;

namespace Phac.Utility
{
    public abstract class Timer
    {
        protected float Time { get; set; }
        protected float InitialTime = 0.0f;
        public bool IsRunning { get; protected set; }
        public float Progress => Time / InitialTime;
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            InitialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            Time = InitialTime;
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
        public void Pause() => IsRunning = true;

        public abstract void Tick(float deltaTime);
    }

    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value)
        {
        }

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
        public bool IsFinished => Time <= 0.0f;
        public void Reset() => Time = InitialTime;
        public void Reset(float initialTime)
        {
            InitialTime = initialTime;
            Reset();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0.0f)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time += deltaTime;
            }
        }

        public void Reset() => Time = 0;
        public float GetTime() => Time;
    }
}