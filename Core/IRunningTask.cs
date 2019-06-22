using Core.ParticlesProviders;

namespace Core
{
    public interface IRunningTask
    {
        void DoUpdate();
        void Start();
        void Stop();
    }
}