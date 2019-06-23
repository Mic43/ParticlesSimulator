using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.RunningTasks
{
    public class SequentialCompositeTask : IRunningTask
    {
        public IEnumerable<IRunningTask> ChildTasks { get; private set; }

        public SequentialCompositeTask(IEnumerable<IRunningTask> childTasks)
        {
            ChildTasks = childTasks ?? throw new ArgumentNullException(nameof(childTasks));
        }

        public SequentialCompositeTask(params IRunningTask[] childTasks)
            : this(childTasks.AsEnumerable())
        {
           
        }
        public void DoUpdate()
        {
            foreach (var runningTask in ChildTasks)
            {
                runningTask.DoUpdate();
            }
        }

        public void Start()
        {
            foreach (var runningTask in ChildTasks)
            {
                runningTask.Start();
            }
        }

        public void Stop()
        {
            foreach (var runningTask in ChildTasks)
            {
                runningTask.Stop();
            }
        }
    }
}