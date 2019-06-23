namespace Core.RunningTasks
{
    public interface IRunningTask
    {
        void DoUpdate();
        void Start();
        void Stop();
    }
}