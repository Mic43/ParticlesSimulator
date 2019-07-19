using System;

namespace Core.RunningTasks
{
    public class ByIntervalPerform : RunningTask
    {
        private TimeSpan _lastGeneration = TimeSpan.Zero;
        public TimeSpan Interval { get; }
        protected  Action ActionToPerform;

        public ByIntervalPerform(TimeSpan interval) : this(interval, () => {})
        {
        }

        public ByIntervalPerform(TimeSpan interval, Action actionToPerform) 
           
        {
            Interval = interval;
            ActionToPerform = actionToPerform ?? throw new ArgumentNullException(nameof(actionToPerform));
            OnUpdate = TryPerformAction;
        }

        protected void TryPerformAction(TimeSpan timeSpan)
        {
            _lastGeneration += timeSpan;

            if (_lastGeneration <= Interval) return;

            ActionToPerform();
            _lastGeneration -= Interval;
        }
    }
}