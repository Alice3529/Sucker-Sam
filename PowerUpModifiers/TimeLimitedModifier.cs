using System;
using System.Diagnostics;
using _Scripts.Player;

namespace _Scripts.PowerUpModifiers
{
    public class TimeLimitedModifier<T> : IPropertyModifier<T>
    {
        private Stopwatch stopwatch;
        private int expireAfter;

        public TimeLimitedModifier()
        {
            stopwatch = Stopwatch.StartNew();
        }
        public void SetExpirationTimeInSec(int vExpireAfter)
        {
            expireAfter = vExpireAfter;
        }

        public virtual void Initialize(T currentValue){}

        public virtual T Apply(T currentValue) => currentValue;

        public bool IsExpired
        {
            get { return (stopwatch.ElapsedMilliseconds / 1000f) > expireAfter; }
        }
        public float TimeToExpire { get { return (float)Math.Round(expireAfter - stopwatch.ElapsedMilliseconds / 1000f, 1); }
            set { expireAfter = (int)value; } }
    }
}