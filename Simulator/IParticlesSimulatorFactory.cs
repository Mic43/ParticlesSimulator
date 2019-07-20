namespace Simulator
{
    public interface IParticlesSimulatorFactory<T> where T : struct
    {
        ParticlesSimulator<T> Create();
    }
}