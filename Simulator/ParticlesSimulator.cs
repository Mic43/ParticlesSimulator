using System;
using Core.RunningTasks;

namespace Simulator
{
    public class ParticlesSimulator<T> : IRunningTask where T : struct 
    {
        public SimulationObjectsCollection<T> ObjectsCollection { get; private set; }
        private readonly IRunningTask _simulatorTask;

        public ParticlesSimulator(IRunningTask simulatorTask, SimulationObjectsCollection<T> objectsCollection)
        {
            _simulatorTask = simulatorTask ?? throw new ArgumentNullException(nameof(simulatorTask));
            ObjectsCollection = objectsCollection ?? throw new ArgumentNullException(nameof(objectsCollection));
        }

        public void DoUpdate()
        {
            _simulatorTask.DoUpdate();
        }

        public void Start()
        {
            _simulatorTask.Start();
        }

        public void Stop()
        {
            _simulatorTask.Stop();
        }

    }
}