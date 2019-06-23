using System;
using System.Diagnostics;

namespace Core.RunningTasks
{

    public class RunningTask : IRunningTask
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public bool IsStarted { get; private set; } = false;
        public Action<TimeSpan> OnUpdate { get; protected set; }
        public TimeSpan TotalTime { get; private set; } = TimeSpan.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onUpdate"> action to execute on each update, with input parameter
        /// of time elapsed since previous call</param>
        public RunningTask(Action<TimeSpan> onUpdate)
        {
            OnUpdate = onUpdate ?? throw new ArgumentNullException(nameof(onUpdate));
        }

        public RunningTask() : this((ts) => { })
        {
           
        }

        public void DoUpdate()
        {         
            if(!IsStarted)
                throw new InvalidOperationException("You must start task first");

            OnUpdate(_stopwatch.Elapsed);

            TotalTime += _stopwatch.Elapsed;
            _stopwatch.Restart();
        }

        public void Start()
        {
            IsStarted = true;
            _stopwatch.Start();
        }

        public void Stop()
        {
            IsStarted = false;
            _stopwatch.Stop();
        }
    }
}