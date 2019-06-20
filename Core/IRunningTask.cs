using Core.ParticlesProviders;

namespace Core
{
    public interface IRunningTask
    {
        void DoUpdate();
        void Start();
        void Stop();
    }

    class ParticlesUpdater : IRunningTask
    {
        private IParticlesWriter writer;

        public void DoUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}